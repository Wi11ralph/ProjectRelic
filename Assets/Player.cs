using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //kqc3yy is a noob
    //M-East is a noob
    //SecSad is a noob
    //TexturephobeCreator is a noob
    //hm

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;

    private Vector3 newPos;
    private Vector3 _input;
    [SerializeField] GameObject Camera;
    private bool isJumpPressed = false;
    private void Start()
    {
        newPos = Camera.transform.position;

    }
    private void Update()
    {
        GatherInput();
        Look();
        Camera.transform.position = newPos + transform.position;
        isJumpPressed = Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
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

    private void Move()
    {
        _rb.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * _speed * Time.deltaTime);
    }
    private void Jump()
    {
        if (isJumpPressed)
        {
            // the cube is going to move upwards in 10 units per second
            _rb.velocity += new Vector3(0, 10, 0);
            Debug.Log("jump");
        }

    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}



