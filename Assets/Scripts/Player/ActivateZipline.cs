using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateZipline : Interaction
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInteraction();
    }
    public override void Use()
    {

        Debug.Log("dziala");
        useObject.GetComponent<ZipLine>().isPlayer = true;
           
    }
}
