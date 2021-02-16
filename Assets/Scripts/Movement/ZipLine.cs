using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipLine : MonoBehaviour
{
    public Transform pos1, pos2;
    public float time;
    public Vector3 lerpPos;
    public bool isPlayer;
    private GameObject playerObject;
    public float moveSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("_Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerObject.GetComponent<Rigidbody>().isKinematic = isPlayer;
        if (isPlayer)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isPlayer = !isPlayer;
                playerObject.GetComponent<Rigidbody>().AddForce(0, 1500, 0, ForceMode.VelocityChange);

                time = 0;
            }

            playerObject.transform.position = lerpPos - new Vector3(0, 1,0);
            time += Time.deltaTime * moveSpeed;
            if (time > 1)
            {
                isPlayer = !isPlayer;
                time = 0;
            }
        }    
        lerpPos = Vector3.Lerp(pos1.position, pos2.position, time);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawCube(lerpPos, new Vector3(1,1,1));
    }
}
