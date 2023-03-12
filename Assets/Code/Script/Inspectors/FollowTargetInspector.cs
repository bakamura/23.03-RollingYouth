#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FollowTarget))]
public class FollowTargetInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FollowTarget script = (FollowTarget)target;
        if(GUILayout.Button("Reposition Camera"))
        {
            script.RepositionCam();
        }
    }
}
#endif