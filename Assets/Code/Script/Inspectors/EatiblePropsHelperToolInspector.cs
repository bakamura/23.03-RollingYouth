#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EatiblePropsHelperTool))]
public class EatiblePropsHelperToolInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EatiblePropsHelperTool script = (EatiblePropsHelperTool)target;
        if (GUILayout.Button("Reposition Props"))
        {
            script.RepositionProps();
        }

        if (GUILayout.Button("Resize Props"))
        {
            script.ResizeProps();
        }
    }
}
#endif