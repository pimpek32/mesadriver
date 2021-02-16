using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleBase : MonoBehaviour
{
    public Animator anim;
    public float CoolDown;
    private float cdown;
    public int animSize;
    public AudioClip hitClip;
    public LayerMask layer;
    public Transform hitbox;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        cdown = CoolDown;    
    }

    // Update is called once per frame
    void Update()
    {
        CoolDown -= Time.deltaTime;
        if(Input.GetButtonDown("Fire1") && CoolDown < 0)
        {
            int rand = Random.Range(1, animSize);
            anim.SetTrigger("attack");
            anim.SetInteger("animSize", rand);
            CoolDown = cdown;
            if (Physics.Raycast(hitbox.position, hitbox.TransformDirection(Vector3.forward), out hit, 1f, layer))
            {
                if(hit.transform.tag == "Interactable")
                {
                    AudioSource.PlayClipAtPoint(hitClip, hit.point);
                }
            }

        }
    }
    
}
