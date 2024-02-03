using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor to show params based on enum type
/// </summary>

[CustomEditor(typeof(CustomAction))]
public class CustomActionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // To get if we changed enum type
        serializedObject.Update();

        CustomAction customAction = (CustomAction)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("actionType"));

        switch (customAction.actionType)
        {
            case CustomAction.ActionType.Move:
                // Displays only move params
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moveParams"));
                break;

            case CustomAction.ActionType.OpenDoor:
                // Displays only open door params
                EditorGUILayout.PropertyField(serializedObject.FindProperty("openDoorParams"));
                break;

            case CustomAction.ActionType.Talk:
                // Displays only talk params
                EditorGUILayout.PropertyField(serializedObject.FindProperty("talkParams"));
                break;
        }
        // Applies any changes made above
        serializedObject.ApplyModifiedProperties();
    }
}
