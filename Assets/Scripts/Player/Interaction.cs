using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public LayerMask layer;
    public GameObject useObject;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateInteraction()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1f, layer))
            {
                if (hit.transform.tag == "Interactable")
                {

                    useObject = hit.transform.gameObject;
                    Use();
                }
                else Debug.Log("nie mozna uzyc dawd");
            }
        }
        
    }
    public virtual void Use()
    {
        
    }
}
