using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AI_MANAGER : MonoBehaviour
{
    public float visiblityStatus = 0f;
    public float searchMargin = 10f;
    public float combatMargin = 60f;
    public Text textIndicator;
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (visiblityStatus < 0) visiblityStatus = 0;
        if(visiblityStatus > combatMargin )
        {
            textIndicator.text = "STATUS: COMBAT";
        }
        if(visiblityStatus > searchMargin && visiblityStatus < combatMargin)
        {
            textIndicator.text = "STATUS: SEARCH";
        }
        if(visiblityStatus < searchMargin)
        {
            textIndicator.text = "STATUS: IDLE";
        }
    }
}
