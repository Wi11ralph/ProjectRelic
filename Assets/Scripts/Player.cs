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
    [SerializeField] private float _turnSpeed = 360;
    [SerializeField] private float _gravity = -2;
    [SerializeField] private float _movementSpeed = 2.8f;
    [SerializeField] private float _jumpHeight = 1.8f;

    [SerializeField] private Clamp xClamp = new(-10, 10);
    [SerializeField] private Clamp zClamp = new(-10, 10);

    [SerializeField] private CharacterController controller;
    [SerializeField] GameObject Camera;

    [CustomEditor(typeof(Player))]
    [CanEditMultipleObjects]
    [HideInInspector]
    public class MyPlayerEditor : Editor
    {
        private SerializedProperty
            turnSpeed,
            gravity,
            movementSpeed,
            jumpHeight,
            xClampMax, zClampMax,
            xClampMin, zClampMin,
            cnrtl, cam;

        private void OnEnable()
        {
            turnSpeed = serializedObject.FindProperty("_turnSpeed");
            gravity = serializedObject.FindProperty("_gravity");
            movementSpeed = serializedObject.FindProperty("_movementSpeed");
            jumpHeight = serializedObject.FindProperty("_jumpHeight");

            xClampMax = serializedObject.FindProperty("xClamp.max");
            zClampMax = serializedObject.FindProperty("zClamp.max");

            xClampMin = serializedObject.FindProperty("xClamp.min");
            zClampMin = serializedObject.FindProperty("zClamp.min");

            cnrtl = serializedObject.FindProperty("controller");
            cam = serializedObject.FindProperty("Camera");
        }
        protected static bool showRef = false;
        protected static bool showClamp = true;
        protected static bool showMovement = true;

        private Vector3 mini, maxi;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //Player p = (Player)target;
            //SerializedProperty ts = serializedObject.FindProperty("_turnSpeed");
            //EditorGUILayout.PropertyField(turnSpeed, new GUIContent("How fast boi SPIN"));
            GUIStyle myFoldoutStyle = new(EditorStyles.foldout);
            myFoldoutStyle.fontStyle = FontStyle.Bold;
            myFoldoutStyle.fontSize = 14;

            showMovement = EditorGUILayout.Foldout(showMovement, "Movement", myFoldoutStyle); 
            if (showMovement)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(gravity);
                EditorGUILayout.PropertyField(jumpHeight);
                EditorGUILayout.PropertyField(movementSpeed);
                EditorGUILayout.PropertyField(turnSpeed);
                EditorGUI.indentLevel--;
            }
            showClamp = EditorGUILayout.Foldout(showClamp, "Camera Clamp", myFoldoutStyle); 
            if (showClamp)
            {
                maxi = new(xClampMax.floatValue, 0f, zClampMax.floatValue);
                mini = new(xClampMin.floatValue, 0f, zClampMin.floatValue);
                
                EditorGUI.indentLevel++;
                maxi = EditorGUILayout.Vector3Field("Maximum", maxi);
                mini = EditorGUILayout.Vector3Field("Minimum", mini);
                EditorGUI.indentLevel--;

                xClampMax.floatValue = maxi.x;
                zClampMax.floatValue = maxi.z;

                xClampMin.floatValue = mini.x;
                zClampMin.floatValue = mini.z;
            }
            showRef = EditorGUILayout.Foldout(showRef, "Refrences", myFoldoutStyle); 
            if (showRef)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(cnrtl);
                EditorGUILayout.PropertyField(cam);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();

        }
    }
    

    private int dJump;
    private Vector3 newPos;
    private Vector3 _input;
    private Vector3 moveDirection;

    private float speedMulti = 1f;
    private float yVelocity;
    private float gravityAcceleration;
    private float jumpSpeed;
    
    private bool isJumpPressed = false;
    private bool isSprintPressed = false;
    private bool waitForFalse = false;
    
    private void Start()
    {
        Initialize();
        newPos = Camera.transform.position;

    }
    private void Update()
    {
        //Debug.Log(newPos); (-4.17, 3.95, -4.15)
        GatherInput();
        Look();
        Move();
        Camera.transform.position = new Vector3(
            Mathf.Clamp(newPos.x + transform.position.x, xClamp.min, xClamp.max),
            newPos.y + transform.position.y,
            Mathf.Clamp(newPos.z + transform.position.z, zClamp.min, zClamp.max)
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

        gravityAcceleration = _gravity * 0.02f * 0.02f;
        jumpSpeed = Mathf.Sqrt(_jumpHeight * -2f * gravityAcceleration);

        if (SceneLoader.pos.y >= 0.5f) transform.position = SceneLoader.pos;
        transform.rotation = SceneLoader.rot;

        Physics.SyncTransforms();
    }
    private void Move()
    {
        isJumpPressed = Input.GetButton("Jump");
        isSprintPressed =  Input.GetKey(KeyCode.LeftShift);

        moveDirection = transform.forward * _input.normalized.magnitude * _movementSpeed * Time.deltaTime;

        if (isSprintPressed && _input.normalized.magnitude != 0) {
            speedMulti = Mathf.Lerp(speedMulti, 2.7f, 0.005f);
            moveDirection *= speedMulti;
            //Debug.Log(speedMulti);
        } else speedMulti = 1f;

        Jump();
        controller.Move(moveDirection);
    }
    private void Jump()
    {
        if (controller.isGrounded)
        {
            dJump = 1;
            yVelocity = 0f;
        }
        
        if (isJumpPressed && !waitForFalse && dJump <= 2)
        {
            yVelocity = jumpSpeed;
            waitForFalse = true;
            dJump++;
        }
        else if (!isJumpPressed) waitForFalse = false;

        yVelocity += gravityAcceleration;

        moveDirection.y = yVelocity;
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}




