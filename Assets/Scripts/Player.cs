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

    //[SerializeField] private Rigidbody _rb;

    public Text debug;
    float _slopeAngle;

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
    [SerializeField] private float _turnSpeed = 360;
    [SerializeField] private float _movementSpeed = 2.8f;
    [SerializeField] private float _jumpHeight = 1.8f;

    [SerializeField] private Clamp xClamp = new(-10, 10);
    [SerializeField] private Clamp zClamp = new(-10, 10);

    [SerializeField] private Rigidbody _rb;
    [SerializeField] GameObject Camera;

    
    

    private int dJump;
    private Vector3 newPos = new (4.17f, 3.95f, -4.15f);
    private Vector3 _input;
    private Vector3 moveVec;
    private Vector3 targetCamPos;

    private float speedMulti = 1f;
    //private float yVelocity;
    //private float gravityAcceleration;
    //private float jumpSpeed;
    private float distToGround = 1f;
    //float _slopeAngle;

    private bool isJumpPressed = false;
    private bool isSprintPressed = false;
    private bool waitForFalse = false;
    private bool isGrounded = false;
    

    private void Start()
    {
        //Cursor.visible = false;
        newPos = Camera.transform.position;

        Debug.Log(Camera.transform.position);
        Initialize();  
    }
    private void FixedUpdate()
    {
        Look();
    }
    private void Update()
    {
        //Debug.Log(Camera.transform.position);
        Debug.Log(newPos);
        //Debug.Log(newPos); (-4.17, 3.95, -4.15)
        GatherInput();
        
        Move();
        GroundCheck();
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

        //gravityAcceleration = _gravity * 0.02f * 0.02f;
        //jumpSpeed = Mathf.Sqrt(_jumpHeight * -2f * gravityAcceleration);

        if (SceneLoader.pos.y >= 0.5f) transform.position = SceneLoader.pos;
        transform.rotation = SceneLoader.rot;
        //Camera.transform.position = SceneLoader.camPos;
        //targetCamPos = SceneLoader.camPos;
        Camera.transform.position = SceneLoader.camPos;
        Physics.SyncTransforms();
    }
    private void GroundCheck()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.001f))
        {
            _slopeAngle = (Vector3.Angle(hit.normal, transform.forward) - 90);
            //debug.text = "Grounded on " + hit.transform.name;
            //debug.text += "\nSlope Angle: " + _slopeAngle.ToString("N0") + "�";
            isGrounded = true;
        }
        else
        {
            //debug.text = "Not Grounded";
            isGrounded = false;
        }
    }
    private void Move()
    {
        if (Pause.active) return;
        isJumpPressed = Input.GetButton("Jump");
        isSprintPressed =  Input.GetKey(KeyCode.LeftShift);

        //moveDirection = transform.forward * _input.normalized.magnitude * _movementSpeed * Time.deltaTime;

        if (isSprintPressed && _input.normalized.magnitude != 0) {
            speedMulti = Mathf.Lerp(speedMulti, 2.7f, 0.005f);
            //Debug.Log(speedMulti);
        } else speedMulti = 1f;


        //_rb.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * speedMulti * _movementSpeed * Time.deltaTime);
        moveVec = transform.forward * _input.normalized.magnitude * speedMulti * _movementSpeed;
        _rb.velocity = new(moveVec.x,_rb.velocity.y,moveVec.z);
        _rb.angularVelocity = new(0f, 0f, 0f);
        //_rb.velocity = new Vector3(0f,_rb.velocity.y,0f);
        Jump();
    }

    private void Jump()
    {
        if (isGrounded)
        {
            dJump = 1;
        }
        if (isJumpPressed && !waitForFalse && dJump <= 1)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpHeight, _rb.velocity.z);
            dJump++;
            waitForFalse = true;
        }
        else if (!isJumpPressed) waitForFalse = false;
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}




