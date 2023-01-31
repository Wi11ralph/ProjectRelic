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

    
    [SerializeField] private float _turnSpeed = 360;
    [SerializeField] private float gravity;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float[] xClamp = {-10,10};
    [SerializeField] private float[] zClamp = {-10,10};
    [SerializeField] private CharacterController controller;

    public Text debug;
    float _slopeAngle;

  

    private Vector3 newPos;
    private Vector3 _input;
    private Vector3 moveDirection;

    private float speedMulti = 1f;
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
        Move();
        Camera.transform.position = new Vector3(
            Mathf.Clamp(newPos.x + transform.position.x, xClamp[0], xClamp[1]),
            newPos.y + transform.position.y,
            Mathf.Clamp(newPos.z + transform.position.z, zClamp[0], xClamp[1])
        );

        //Debug.Log(_input);
    }

    private void FixedUpdate()
    {
        
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
        gravityAcceleration = gravity * Time.deltaTime * Time.deltaTime;
        jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * gravityAcceleration);
    }
    private void Move()
    {
        isJumpPressed = Input.GetButton("Jump");
        isSprintPressed =  Input.GetKey(KeyCode.LeftShift);

        moveDirection = transform.forward * _input.normalized.magnitude * movementSpeed * Time.deltaTime;

        if (isSprintPressed && _input.normalized.magnitude != 0) {
            speedMulti = Mathf.Lerp(speedMulti, 2.7f, 0.005f);
            moveDirection *= speedMulti;
            Debug.Log(speedMulti);
        } else speedMulti = 1f;

        Jump();
        controller.Move(moveDirection);
    }
    private void Jump()
    {
        if (controller.isGrounded)
        {
            yVelocity = 0f;
            if (isJumpPressed)
                yVelocity = jumpSpeed;
        }
        yVelocity += gravityAcceleration;

        moveDirection.y = yVelocity;
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}




