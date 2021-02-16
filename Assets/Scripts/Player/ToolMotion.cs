using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMotion : MonoBehaviour
{   public Vector2 sensivity;
    public Vector2 clamp;
    public float time;
    Vector2 velocity, interpolated, interpolationVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector2(Input.GetAxis("Mouse Y")  * sensivity.x + (Input.GetAxis("Vertical") * 1f), Input.GetAxis("Mouse X") * sensivity.y + (Input.GetAxis("Horizontal") * 2f));
        interpolated = Vector2.SmoothDamp(interpolated, velocity, ref interpolationVelocity, time);
        interpolated = new Vector2(Mathf.Clamp(interpolated.x, -clamp.x, clamp.x), Mathf.Clamp(interpolated.y, -clamp.y, clamp.y));
        transform.localEulerAngles = new Vector3(interpolated.x, -interpolated.y,0);
    }
}
