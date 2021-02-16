using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pistol : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public float CoolDown = 0.5f;
    private float bulletSpeed;
    private float cdown;
    public GameObject bullet;
    public Transform barrel;
    public LayerMask layer;
    public Transform laserSight;
    private Vector3 hitpoint;
    public Transform Cam;
    private AudioSource audioSource;
    RaycastHit hit;
    [Header("Reload")] 
    public int MaxAmmo = 100;
    public int TotalAmmo = 100;
    public int ClipSize = 15;
    public int CurrentAmmo = 15; // full clip on start
    public float reloadTime = 1F; 
    bool reloading;
    [Header("Sounds")]
    public AudioClip empty;
    public AudioClip shoot;
    public AudioClip reload;
    public AudioClip noAmmo;
    public Text ammoText;
    public GameObject muzzleFlash;

    void Start()
    {
        cdown = CoolDown;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Cam.position, Cam.TransformDirection(Vector3.forward), out hit, layer))
        {
            laserSight.position = hit.point;
            hitpoint = hit.point;
        }
        CoolDown -= Time.deltaTime;
       if(Input.GetButtonDown("Fire1") && CoolDown < 0)
        {
            Shoot();
        }
       if(Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(CR_Reload());

        }
        ammoText.text = CurrentAmmo + "\n" + TotalAmmo;
    }
    
    void Shoot()
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        if(!reloading && CurrentAmmo > 0)
        {
            CurrentAmmo--; 
            GameObject muz = Instantiate(muzzleFlash, barrel.position, Quaternion.identity);
            muz.transform.parent = barrel;
            Destroy(muz, 3);
            audioSource.PlayOneShot(shoot);
            anim.SetTrigger("shoot");
            CoolDown = cdown;
            GameObject obj;
            obj = Instantiate(bullet, Cam.position, Quaternion.identity);
            obj.GetComponent<Bullet>().velocity = Cam.TransformDirection(new Vector3(/*Random.Range(-0.015f, 0.015f)*/0, 0/*Random.Range(-0.015f, 0.015f)*/, 1));
        }

        if (CurrentAmmo <= 0)
            audioSource.PlayOneShot(empty);
    }
    IEnumerator CR_Reload()
    {

        if (reloading == false)
        {

            if (TotalAmmo == 0)
            { // Ammo depleted, no reason to go on
                reloading = false;
                audioSource.PlayOneShot(noAmmo);
                yield break;
            }
            else if (CurrentAmmo == ClipSize)
            {
                reloading = false;
                audioSource.PlayOneShot(noAmmo);
                yield break;
            }
            else if (TotalAmmo < ClipSize)
            { // Don't reload a complete clip if there are only a few bullets left
                int temp = CurrentAmmo;
                CurrentAmmo = TotalAmmo;
                TotalAmmo = temp;
            }
            else // Else simply assign the clip size
            {
                TotalAmmo -= Mathf.Clamp(ClipSize - CurrentAmmo, 0, MaxAmmo);
                CurrentAmmo = ClipSize;
            }
            reloading = true;
        }
       // if(reloading == true)
        anim.SetBool("reload", true);
        audioSource.PlayOneShot(reload);
        yield return new WaitForSeconds(reloadTime); // Wait until animation is done
        anim.SetBool("reload", false);
       // TotalAmmo -= Mathf.Clamp(ClipSize - CurrentAmmo, 0, MaxAmmo);
        reloading = false;
    }
}
