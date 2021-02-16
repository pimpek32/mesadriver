using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 lastPos;
    public Vector3 curPos;
    public Vector3 firstPos;
    [HideInInspector]
    public Vector3 velocity;
    public float bulletDisappearDistance;
    public float bulletDistance;
    public float curDistance;
    public float bulletSpeed;
    public float bulletFalloff = 1;
    public LayerMask layers;
    public GameObject debugCol;
    public GameObject wound;
   // public GameObject bloodSplat;
    private Rigidbody rb;
    RaycastHit hit;
    private Vector3 vel2;
    // Start is called before the first frame update
    void Start()
    {
        firstPos = transform.position;
        lastPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //curPos = transform.position;
        curDistance = Vector3.Distance(transform.position, firstPos);
        //transform.Translate(new Vector3(0, -Mathf.Exp(curDistance / bulletDistance) * bulletFalloff + bulletFalloff, 0) * Time.deltaTime);
        /*- new Vector3(0, Mathf.Exp(curDistance / bulletDistance) * bulletFalloff + bulletFalloff,0)*/
       // vel2 = new Vector3(0, -Mathf.Exp(curDistance / bulletDistance), 0);
      //  transform.Translate((velocity * bulletSpeed) * Time.deltaTime);
        if (Physics.Linecast(lastPos, transform.position, out hit, layers))
        {


           // GameObject splat;
            GameObject obj;
            if (hit.transform.tag == "Enemy")
            {
                obj = Instantiate(wound, hit.point, Quaternion.LookRotation(hit.normal));
               // splat = Instantiate(bloodSplat, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                obj = Instantiate(debugCol, hit.point, Quaternion.LookRotation(hit.normal));
                if (hit.transform.GetComponent<Rigidbody>())
                {
                    hit.transform.GetComponent<Rigidbody>().AddForce(-hit.normal * 750f);
                }
            }
            obj.transform.parent = hit.transform;
            Destroy(obj, 20);
            Destroy(gameObject);
        }
        if (Vector3.Distance(transform.position, firstPos) > bulletDisappearDistance) Destroy(gameObject);
        curPos = transform.position;


        rb.AddForce(velocity * bulletSpeed);
    }

    private void FixedUpdate()
    {
    }
    private void LateUpdate()
    {
        lastPos = transform.position;
    }
    
}
