﻿Shader "Hidden/Custom/AO"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
			Tags{ "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}

            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
				
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
            TEXTURE2D_ARRAY_FLOAT(_CameraDepthTexture);
#else
            TEXTURE2D_FLOAT(_CameraDepthTexture);
#endif
            SAMPLER(sampler_CameraDepthTexture);

			TEXTURE2D(_NoiseTex);
			SAMPLER(sampler_NoiseTex);

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

			#define SAMPLE_DEPTH_AO(uv) LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r, _ZBufferParams);

			float4 _NoiseTex_TexelSize;

			//Common Settings
			half _AO_Intensity;
			half _AO_Radius;

			// SSAO Settings
			int _SSAO_Samples;
			float _SSAO_Area;

			struct Attributes
			{
				float4 positionOS   : POSITION;
				float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct Varyings
			{
				half4  positionCS   : SV_POSITION;
				half2  uv           : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
			};

			float3 NormalFromDepth(float depth, float2 texcoords) 
			{
				const float2 offset = float2(0.001,0.0);

				float depthX = SAMPLE_DEPTH_AO(texcoords + offset.xy);
				float depthY = SAMPLE_DEPTH_AO(texcoords + offset.yx);

				float3 pX = float3(offset.xy, depthX - depth);
				float3 pY = float3(offset.yx, depthY - depth);

				float3 normal = cross(pY, pX);
				normal.z = -normal.z;

				return normalize(normal);

			}

			float SSAO(float2 coords) {

				float area = _SSAO_Area;
				const float falloff = 0.05;
				float radius = _AO_Radius;

				const int samples = _SSAO_Samples;
				const float3 sample_sphere[16] = {
				float3( 0.5381, 0.1856,-0.4319), float3( 0.1379, 0.2486, 0.4430),
				float3( 0.3371, 0.5679,-0.0057), float3(-0.6999,-0.0451,-0.0019),
				float3( 0.0689,-0.1598,-0.8547), float3( 0.0560, 0.0069,-0.1843),
				float3(-0.0146, 0.1402, 0.0762), float3( 0.0100,-0.1924,-0.0344),
				float3(-0.3577,-0.5301,-0.4358), float3(-0.3169, 0.1063, 0.0158),
				float3( 0.0103,-0.5869, 0.0046), float3(-0.0897,-0.4940, 0.3287),
				float3( 0.7119,-0.0154,-0.0918), float3(-0.0533, 0.0596,-0.5411),
				float3( 0.0352,-0.0631, 0.5460), float3(-0.4776, 0.2847,-0.0271)
				};

				//Random Noise vector
				float3 random = normalize(SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, coords * ( _ScreenParams.xy / _NoiseTex_TexelSize.zw)).rgb);

				//Depth texture
				float depth = SAMPLE_DEPTH_AO(coords);

				float3 position = float3(coords, depth);

				// Reconstruct normals
				float3 normal = NormalFromDepth(depth, coords);

				float radius_depth = radius;///depth;
				float occlusion = 0.0;

				for(int i=0; i < samples; i++) {

					float3 ray = radius_depth * reflect(sample_sphere[i], random);
					float3 hemi_ray = position + sign(dot(ray,normal)) * ray;

					float occ_depth = SAMPLE_DEPTH_AO(saturate(hemi_ray.xy));
					float difference = depth - occ_depth;

					occlusion += step(falloff, difference) * (1.0-smoothstep(falloff, area, difference));

				}

				float ao = 1.0 - _AO_Intensity * occlusion * (1.0 / samples);

				return ao;
			}

			Varyings Vertex(Attributes input) {
			
				Varyings output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				output.positionCS = TransformObjectToHClip(input.positionOS.xyz);

				output.uv = UnityStereoTransformScreenSpaceTex(input.texcoord);

				return output;
            }

			half4 Fragment(Varyings input) : SV_Target {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
				float ao = SSAO(input.uv);
				return col *= ao;
			}

            ENDHLSL
        }
    }
}

