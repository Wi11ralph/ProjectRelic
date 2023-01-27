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
    [SerializeField] private float gravity;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private CharacterController controller;

    public Text debug;
    float _slopeAngle;


    private Vector3 newPos;
    private Vector3 _input;

    private float distToGround = 1f;
    private bool isGrounded = false;
    private float yVelocity;
    private float gravityAcceleration;
    private float jumpSpeed;

    [SerializeField] GameObject Camera;
    private bool isJumpPressed = false;
    private bool isSprintPressed = false;
    private void Start()
    {
        Initialize();
        newPos = Camera.transform.position;
    }
    private void Update()
    {
        GatherInput();
        Look();
        Camera.transform.position = newPos + transform.position;
        
        Debug.Log(_input);
    }

    private void FixedUpdate()
    {
        Move();
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
    private void Initialize()
    {
        gravityAcceleration = gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
        jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * gravityAcceleration);
    }
    private void Move()
    {
        isJumpPressed = Input.GetButtonDown("Jump");
        //isSprintPressed = Input.GetButtonDown("Sprint");

        Vector3 moveDirection = transform.forward * _input.normalized.magnitude * _speed * Time.deltaTime;

        if (isSprintPressed)
            moveDirection *= 2f;

        if (controller.isGrounded)
        {
            yVelocity = 0f;
            if (isJumpPressed)
                yVelocity = jumpSpeed;
        }
        yVelocity += gravityAcceleration;

        moveDirection.y = yVelocity;
        controller.Move(moveDirection);
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



