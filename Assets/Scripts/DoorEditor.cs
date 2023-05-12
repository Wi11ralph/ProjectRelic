#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
[CustomEditor(typeof(DoorController))]
[CanEditMultipleObjects]
[HideInInspector]
public class DoorEditor : Editor
{
    private SerializedProperty _id, _startPos, _openPos, ot, ct,booll,rf,p;

    private void OnEnable()
    {
        _id = serializedObject.FindProperty("id");
        booll = serializedObject.FindProperty("keyMode");
        _startPos = serializedObject.FindProperty("startPos");
        _openPos = serializedObject.FindProperty("openPos");
        ot = serializedObject.FindProperty("openTime");
        ct = serializedObject.FindProperty("closeTime");
        rf = serializedObject.FindProperty("outline");
        p = serializedObject.FindProperty("player");
    }
    protected static bool showPos = true, showTime = true, reff =  false;
    public override void OnInspectorGUI()
    {
        //DoorController door = (DoorController)target;

        serializedObject.Update();

        GUIStyle myFoldoutStyle = new(EditorStyles.foldout);
        myFoldoutStyle.fontStyle = FontStyle.Bold;
        myFoldoutStyle.fontSize = 14;

        GUIStyle Bold = new(EditorStyles.label);
        Bold.fontStyle = FontStyle.Bold;
        Bold.fontSize = 14;

        //EditorGUILayout.PropertyField(_id, new GUIContent("ID"));
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("ID", Bold);
        _id.intValue = EditorGUILayout.IntField(_id.intValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(booll, new GUIContent("Key mode?"));

        showPos = EditorGUILayout.Foldout(showPos, "Position", myFoldoutStyle);
        if (showPos)
        {
            EditorGUI.indentLevel++;
            _startPos.vector3Value = EditorGUILayout.Vector3Field("Starting Position", _startPos.vector3Value);
            _openPos.vector3Value = EditorGUILayout.Vector3Field("Open Position", _openPos.vector3Value);
            EditorGUI.indentLevel--;
        }
        showTime = EditorGUILayout.Foldout(showTime, "Animation time", myFoldoutStyle);
        if (showTime)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(ot, new GUIContent("Opening time"));
            EditorGUILayout.PropertyField(ct, new GUIContent("Closing time"));
            EditorGUI.indentLevel--;
        }
        if (booll.boolValue)
        {
            reff = EditorGUILayout.Foldout(reff, "Refrences", myFoldoutStyle);
            if (reff)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(rf, new GUIContent("Outline material"));
                EditorGUILayout.PropertyField(p, new GUIContent("Player"));
                EditorGUI.indentLevel--;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

}
#endif