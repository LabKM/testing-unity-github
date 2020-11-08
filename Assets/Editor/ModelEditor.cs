using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextMeshWriter))]
public class ModelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TextMeshWriter converter = target as TextMeshWriter;
        if (GUILayout.Button("Convert"))
        {
            converter.Convert();
        }
    }
}
