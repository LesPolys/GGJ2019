#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(Furniture))]
public class FurnitureEditor : Editor
{
    public override void OnInspectorGUI()
    {

        Furniture myScript = (Furniture)target;
        if (GUILayout.Button("Set Final Values"))
        {
            myScript.SetFinalValues();
        }
        DrawDefaultInspector();

    }
}
#endif
