#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        cnrtl, cam, rb, grn;

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

        //cnrtl = serializedObject.FindProperty("controller");
        cam = serializedObject.FindProperty("Camera");
        rb = serializedObject.FindProperty("_rb");
        grn = serializedObject.FindProperty("groundCheck");
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
            //EditorGUILayout.PropertyField(gravity);
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
            EditorGUILayout.PropertyField(rb);
            //EditorGUILayout.PropertyField(cnrtl);
            EditorGUILayout.PropertyField(cam);
            EditorGUILayout.PropertyField(grn);
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();

    }
}
#endif
