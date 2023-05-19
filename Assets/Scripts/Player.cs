using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Player : MonoBehaviour
{
    //kqc3yy is a noob
    //M-East is a noob
    //SecSad is a noob
    //TexturephobeCreator is a noob
    //hm
    [SerializeField] private Animator anim;
    
    
    //[SerializeField] private Rigidbody _rb;

    public Text debug;
    //float _slopeAngle;

    [System.Serializable]
    public class Clamp
    {
        public float min,max;

        public Clamp(float minn, float maxx)
        {
            min = minn;
            max = maxx;
        }
    }
    [System.Serializable]
    private class Sounds
    {
        public AudioSource audioSource;
        public AudioSource feetSource;
        public AudioSource sprintSource;
        public AudioClip jump;
        public AudioClip land;
        public AudioClip footsteps;
    }
    [SerializeField] private Sounds sounds;

    public static bool fireRelic = false;
    public static bool airRelic = false;
    public static bool natureRelic = false;

    public List<int> keys;

    private float jumpWaiter = 0;
    private RaycastHit hit;
    private float transformPoint;

    [SerializeField] private float height;
    [SerializeField] private float _turnSpeed = 360;
    [SerializeField] private float _movementSpeed = 2.8f;
    [SerializeField] private float _jumpHeight = 1.8f;

    [SerializeField] private Clamp xClamp = new(-10, 10);
    [SerializeField] private Clamp zClamp = new(-10, 10);

    [SerializeField] private Rigidbody _rb;
    [SerializeField] GameObject Camera;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private GameObject shadowPoint;

    private Player cb;
    private int dJump;
    private Vector3 newPos = new (4.17f, 3.95f, -4.15f);
    private Vector3 _input;
    private Vector3 moveVec;
    private Vector3 targetCamPos;

    private float speedMulti = 1f;
    //private float yVelocity;
    //private float gravityAcceleration;
    //private float jumpSpeed;
    //private float distToGround = 0.5f;
    //float _slopeAngle;

    private bool isJumpPressed = false;
    private bool isSprintPressed = false;
    private bool waitForFalse = false;
    private bool isGrounded = false;
    private bool zooming = false;
    public bool IsGrappling;
    private void Start()
    {

        //Cursor.visible = false;
        
        if (this.tag != "Player") return;
        newPos = Camera.transform.position;
        Collider collider = groundCheck.GetComponent<Collider>();
        cb = collider.gameObject.AddComponent<Player>();
        //cb.Initialize();

        Initialize();  
    }
    private bool firstFrame = true;
    private void LateUpdate()
    {
        if (!firstFrame) return;
        firstFrame = false;
    }
    private void FixedUpdate()
    {
        if (this.tag != "Player") return;
        Look();
    }
    private bool waitForLand = false;
    private bool steps = true;
    private bool stomps = true;
    private void Update()
    { 
        if (firstFrame) return;
        //Debug.Log(Camera.transform.position);
        if (this.tag != "Player") return;

        if (waitForLand && groundCheck.GetComponent<Player>().isGrounded)
        {
            waitForLand = false;
            sounds.audioSource.PlayOneShot(sounds.land);
        }
        if (!groundCheck.GetComponent<Player>().isGrounded) waitForLand = true;
        //Debug.Log(newPos); (-4.17, 3.95, -4.15)
        GatherInput();
        
        Move();
        //GroundCheck();
        targetCamPos = new Vector3(
            Mathf.Clamp(newPos.x + transform.position.x, xClamp.min, xClamp.max),
            newPos.y + transform.position.y,
            Mathf.Clamp(newPos.z + transform.position.z, zClamp.min, zClamp.max)
        );

        Camera.transform.position = new(
            Mathf.Lerp(Camera.transform.position.x, targetCamPos.x, 1.2f * Time.unscaledDeltaTime),
            Mathf.Lerp(Camera.transform.position.y, targetCamPos.y, 4.5f * Time.unscaledDeltaTime),
            Mathf.Lerp(Camera.transform.position.z, targetCamPos.z, 1.2f * Time.unscaledDeltaTime)
        );
        if(zooming)
        {
            Camera.GetComponentInChildren<Camera>().orthographicSize = Mathf.Lerp(
                Camera.GetComponentInChildren<Camera>().orthographicSize,
                SceneLoader.pzoom,
                1.2f * Time.unscaledDeltaTime
            );
        }
        float speedd = _input.normalized.magnitude * speedMulti * _movementSpeed;
        anim.SetFloat("speed", speedd);
        if (speedd > 0.1f && speedd < 3f && groundCheck.GetComponent<Player>().isGrounded)
        {
            sounds.audioSource.loop = true;
            if (steps)
            {
                //sounds.feetSource.clip = sounds.footsteps;
                sounds.feetSource.Play();
                steps = false;
            }
            sounds.sprintSource.Stop();
            stomps = true;
        }
        else if(speedd > 3 && groundCheck.GetComponent<Player>().isGrounded) {
            if (stomps)
            {
                sounds.sprintSource.Play();
                stomps = false;
            }
            try
            {
                sounds.feetSource.Pause();
                sounds.feetSource.time = Mathf.Min(Mathf.Round(sounds.feetSource.time * 2) / 2, 1.9f);
            }
            catch (System.NullReferenceException) { }
            steps = true;

        }
        else
        {
            try
            {
                sounds.feetSource.Pause(); 
                sounds.feetSource.time = Mathf.Min(Mathf.Round(sounds.feetSource.time * 2)/2,1.9f);
            }
            catch (System.NullReferenceException) { }
            steps = true;
            sounds.sprintSource.Stop();
            stomps = true;
        }
        //Debug.Log(_input.normalized.magnitude * speedMulti * _movementSpeed);
        anim.SetBool("isGrounded",groundCheck.GetComponent<Player>().isGrounded);
        anim.SetBool("grappling", IsGrappling);
        //Debug.Log(_input);
    }

    private void GatherInput()
    {
        if (Pause.active) return;
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.fixedDeltaTime);
    }
    private void Initialize()
    {
        if (this.tag != "Player") return;
        //gravityAcceleration = _gravity * 0.02f * 0.02f;
        //jumpSpeed = Mathf.Sqrt(_jumpHeight * -2f * gravityAcceleration);

        if (SceneLoader.pos.y >= 0.5f) transform.position = SceneLoader.pos;
        Debug.Log(SceneLoader.spawnT + " " + SceneLoader.pos);
        transform.rotation = SceneLoader.rot;
        //Camera.transform.position = SceneLoader.camPos;
        //targetCamPos = SceneLoader.camPos;
        if (SceneLoader.spawnT != SceneLoader.SpawnType.nextLevel) Camera.transform.position = SceneLoader.camPos;
        else
        {
            Camera.transform.position = SceneLoader.camOffset + transform.position;
            zooming = true;
            Camera.GetComponentInChildren<Camera>().orthographicSize = SceneLoader.currentZoom;
        }
        Physics.SyncTransforms();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (this.tag == "Player" || other.tag != "mapElements" && other.tag != "burnable") return; 
        isGrounded = true; 
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (this.tag == "Player" || other.tag == "Player") return;
        isGrounded = false;
        waitForLand = true;
    }
    private void Move()
    {
        if (Pause.active) return;
        isJumpPressed = Input.GetButton("Jump");
        isSprintPressed =  Input.GetKey(KeyCode.LeftShift);

        //moveDirection = transform.forward * _input.normalized.magnitude * _movementSpeed * Time.deltaTime;

        if (isSprintPressed && _input.normalized.magnitude != 0) {
            speedMulti = Mathf.Lerp(speedMulti, 2.7f, 2.5f * Time.deltaTime);
            //Debug.Log(speedMulti);
        } else speedMulti = 1f;


        //_rb.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * speedMulti * _movementSpeed * Time.deltaTime);
        moveVec = transform.forward * _input.normalized.magnitude * speedMulti * _movementSpeed;
        if (!IsGrappling) _rb.velocity = new(moveVec.x, _rb.velocity.y, moveVec.z);
        else _rb.AddForce(moveVec * 5f);
        _rb.angularVelocity = new(0f, 0f, 0f);
        //_rb.velocity = new Vector3(0f,_rb.velocity.y,0f);

        //shadow logic
        Ray ray = new Ray(transform.position, -Vector3.up); 
        Debug.DrawRay(transform.position, Vector3.down * 20f, Color.blue);
        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "mapElements") 
            transformPoint = Mathf.Lerp(shadowPoint.transform.position.y, hit.point.y, 20f * Time.deltaTime);
        if (Physics.Raycast(ray, out hit) && hit.collider.tag == "burnable")
            transformPoint = Mathf.Lerp(shadowPoint.transform.position.y, hit.point.y, 9f * Time.deltaTime);

        shadowPoint.transform.position = new (transform.position.x, transformPoint, transform.position.z);
        //shadow logic

        Jump();
    }

    private void Jump()
    { 
        jumpWaiter -= 1 * Time.unscaledDeltaTime;
        if (cb.isGrounded) if(jumpWaiter < 0) dJump = 1; 
        if (isJumpPressed && !waitForFalse && dJump <= 1 + System.Convert.ToInt32(airRelic))
        {
            sounds.audioSource.PlayOneShot(sounds.jump);
            jumpWaiter = 0.2f;
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpHeight, _rb.velocity.z);
            if (dJump == 2) anim.SetTrigger("dj");
            dJump++; 
            waitForFalse = true;
        }
        else if (!isJumpPressed) waitForFalse = false;
        anim.SetBool("isJumping", isJumpPressed);
        //Debug.Log(anim.GetBool("isJumping"));
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}




