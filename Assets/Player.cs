using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //kqc3yy is a noob
    //M-East is a noob
    //SecSad is a noob
    //TexturephobeCreator is a noob
    //hm

    //[SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;

    public Text debug;
    float _slopeAngle;

    private Vector3 newPos;
    private Vector3 _input;
    private float distToGround = 1f;
    private float distToX = 1f;
    private float distToZ = 1f;
    private bool isGrounded = false;

    [SerializeField] GameObject Camera;
    private bool isJumpPressed = false;
    private void Start()
    {
        newPos = Camera.transform.position;
        distToGround = GetComponent<Collider>().bounds.extents.y;
        distToX = GetComponent<Collider>().bounds.extents.x;
        distToZ = GetComponent<Collider>().bounds.extents.z;
    }
    private void Update()
    {
        GatherInput();
        Look();
        Camera.transform.position = newPos + transform.position;
        isJumpPressed = Input.GetButtonDown("Jump");
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
        GroundCheck();
        Debug.Log(_input);
    }

    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    private void GroundCheck()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.1f))
        {
            _slopeAngle = (Vector3.Angle(hit.normal, transform.forward) - 90);
            //debug.text = "Grounded on " + hit.transform.name;
            //debug.text += "\nSlope Angle: " + _slopeAngle.ToString("N0") + "°";
            isGrounded = true;
        }
        else
        {
            //debug.text = "Not Grounded";
            isGrounded = false;
        }
    }

    private void WallCheck()
    {
        if(place_meeting(new Vector3(1,0,0), distToX + _speed))
        {

        }

    }

    private bool place_meeting(Vector3 angle, float waht)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, angle, out hit, waht)) return true; //distToX + _speed
        else return false;
    }

    private void Move()
    {
        //_rb.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * _speed * Time.deltaTime);
        transform.position += transform.forward * _input.normalized.magnitude * _speed * Time.deltaTime;
    }
    private void Jump()
    {
        if (isJumpPressed && isGrounded)
        {
            // the cube is going to move upwards in 10 units per second
            //_rb.velocity = new Vector3(0, 8, 0);
            Debug.Log("jump");
        }

    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}



