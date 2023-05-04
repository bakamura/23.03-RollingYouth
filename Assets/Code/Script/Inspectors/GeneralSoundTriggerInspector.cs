#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GeneralSoundTrigger))]
[CanEditMultipleObjects]
public class GeneralSoundTriggerInspector : Editor
{
    //general variables
    SerializedProperty soundType, sfxSoundInterval;
    //trigger enter variables
    SerializedProperty playSoundOnTriggerEnter, musicConfigOnTriggerEnter, sfxConfigOnTriggerEnter;
    //trigger exit variables
    SerializedProperty playSoundOnTriggerExit, musicConfigOnTriggerExit, sfxConfigOnTriggerExit;
    //collision enter variables
    SerializedProperty playSoundOnCollisionEnter, musicConfigOnCollisionEnter, sfxConfigOnCollisionEnter;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(soundType, new GUIContent("Sound Type"));

        EditorGUILayout.PropertyField(playSoundOnTriggerEnter, new GUIContent("Play On Trigger Enter"));
        EditorGUILayout.PropertyField(playSoundOnTriggerExit, new GUIContent("Play On Trigger Exit"));
        EditorGUILayout.PropertyField(playSoundOnCollisionEnter, new GUIContent("Play On Collision Enter"));

        if (soundType.enumValueIndex == (int)SoundManager.SoundTypes.SFX)
        {
            EditorGUILayout.PropertyField(sfxSoundInterval, new GUIContent("Sfx Sound Interval", "the interval between the sounds"));

            if (playSoundOnTriggerEnter.boolValue)
            {
                EditorGUILayout.PropertyField(sfxConfigOnTriggerEnter, new GUIContent("Sfx Sounds Cofigurations"));
            }

            if (playSoundOnTriggerExit.boolValue)
            {
                EditorGUILayout.PropertyField(sfxConfigOnTriggerExit, new GUIContent("Sfx Sounds Cofigurations"));
            }

            if (playSoundOnCollisionEnter.boolValue)
            {
                EditorGUILayout.PropertyField(sfxConfigOnCollisionEnter, new GUIContent("Sfx Sounds Cofigurations"));
            }
        }
        //the trigger activates music
        else
        {
            if (playSoundOnTriggerEnter.boolValue)
            {
                EditorGUILayout.PropertyField(musicConfigOnTriggerEnter, new GUIContent("Music Sounds Cofigurations"));
            }
            if (playSoundOnTriggerExit.boolValue)
            {
                EditorGUILayout.PropertyField(musicConfigOnTriggerExit, new GUIContent("Music Sounds Cofigurations"));
            }
            if (playSoundOnCollisionEnter.boolValue)
            {
                EditorGUILayout.PropertyField(musicConfigOnCollisionEnter, new GUIContent("Music Sounds Cofigurations"));
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void OnEnable()
    {
        //general variables
        soundType = serializedObject.FindProperty("_soundType");
        sfxSoundInterval = serializedObject.FindProperty("_sfxSoundInterval");
        //trigger enter
        playSoundOnTriggerEnter = serializedObject.FindProperty("_playSoundOnTriggerEnter");
        musicConfigOnTriggerEnter = serializedObject.FindProperty("_musicConfigOnTriggerEnter");
        sfxConfigOnTriggerEnter = serializedObject.FindProperty("_sfxConfigOnTriggerEnter");
        //trigger exit
        playSoundOnTriggerExit = serializedObject.FindProperty("_playSoundOnTriggerExit");
        musicConfigOnTriggerExit = serializedObject.FindProperty("_musicConfigOnTriggerExit");
        sfxConfigOnTriggerExit = serializedObject.FindProperty("_sfxConfigOnTriggerExit");
        //collision enter
        playSoundOnCollisionEnter = serializedObject.FindProperty("_playSoundOnCollisionEnter");
        musicConfigOnCollisionEnter = serializedObject.FindProperty("_musicConfigOnCollisionEnter");
        sfxConfigOnCollisionEnter = serializedObject.FindProperty("_sfxConfigOnCollisionEnter");
    }
}
#endif