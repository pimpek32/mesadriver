using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    CharacterController characterPhysics;
    public float mouseSensivity = 2f;
    private GameObject GameCamera;
    public float speed = 6.0f;
    public float crouchSpeed = 4.0f;
    public float flySpeed = 2f;
    public float slideSpeed = 1.1f;
    public float crouchAcceleration;
    public float acceleration;
    public float drag;
    public bool crouch;
    public float maxRot = 70f;
    public float gravity = 20f;
    public float jumpForce = 300f;
    public float curRot; //current rotation
    public float curRot1;
    private float reffloat = 0f;
    private Vector2 input;
    public LayerMask layer; //kolizje ktore wykrywa promien sprawdzajacy polozenie gracza;
    public GameObject gameView;
    private Vector3 hitNormal;

    public float slopeLimit;

    float interpolationTime = 0;
    [Header("Climb")]

    [SerializeField]
    private Transform climbTorso, climbHead;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    public bool charMoving()
    {
        return Input.GetButton("Horizontal") || Input.GetButton("Vertical");
    }
    void Start()
    {
        Application.targetFrameRate = 300;
        GameCamera = transform.Find("Camera").gameObject;
      //  gameView = transform.Find("Camera/Game_View").gameObject;
        characterPhysics = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public bool showSlide;
    public bool isSliding()
    {
        return Vector3.Angle(Vector3.up, hitNormal) <= slopeLimit && characterPhysics.isGrounded && crouch && !Physics.Raycast(transform.position + new Vector3(0, 0.35f,0), transform.TransformDirection(moveDirection) * 0.55f, layer) && input.magnitude > 0.5f;

    }
    void Update()
    {
        showSlide = isSliding();

            gameView.GetComponent<Animator>().SetBool("jump", !characterPhysics.isGrounded && characterPhysics.velocity.magnitude > 0.1f);
        gameView.GetComponent<Animator>().SetBool("moving", charMoving());

        crouch = Input.GetButton("Crouch");

        curRot1 += -mouseSensivity * Input.GetAxis("Mouse Y");
        curRot = Mathf.Clamp(curRot1, -maxRot, maxRot);
        curRot1 = Mathf.Clamp(curRot1, -maxRot, maxRot);

        transform.eulerAngles += new Vector3(0, mouseSensivity * Input.GetAxis("Mouse X"), 0);
        GameCamera.transform.eulerAngles = new Vector3(curRot, GameCamera.transform.eulerAngles.y, GameCamera.transform.eulerAngles.z);
        GameCamera.transform.localPosition = new Vector3(0, characterPhysics.height - 0.1f, 0);
        if (transform.eulerAngles.y > 360) transform.eulerAngles -= new Vector3(0, 360, 0);
        
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input.Normalize();

        moveDirection = new Vector3(input.x, 0.0f, input.y);
        velocity += moveDirection * (crouch ? crouchAcceleration : acceleration);
        if (characterPhysics.isGrounded)
        {
            characterPhysics.height = (crouch ? 1.3f : 1.8f);

            characterPhysics.radius = (crouch ? 0.3f : 0.5f);
            if (Input.GetButtonDown("Jump") || Mathf.Abs(Input.GetAxisRaw("Mouse ScrollWheel")) > 0.01f)
            {
                velocity.y = jumpForce;
            }
        }

        velocity.x = velocity.x / drag;
        velocity.z = velocity.z / drag; 
        // Apply gravity
        velocity.y = velocity.y - (gravity * Time.deltaTime);
        // Move the controller
        characterPhysics.Move(transform.TransformDirection(velocity) * Time.deltaTime);

        drag = Mathf.SmoothDamp(drag,
           //  DRAG CALCULATION PRIORITY : isSliding() > isGrounded > isCrouching
           isSliding() ? slideSpeed : (characterPhysics.isGrounded ? (crouch ? crouchSpeed : speed) : flySpeed)
            , ref reffloat, 1f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position + new Vector3(0,1,0), new Vector3(1, 2, 1));
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

}
