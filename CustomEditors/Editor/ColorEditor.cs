using UnityEngine;
using UnityEditor; // must include this

/// <summary>
/// Creates custom editor for the CharacterColorChanger script
/// </summary>

// Must be typeof Whatever script we want to customize editor for and extends Editor class
[CustomEditor(typeof(CharacterColorChanger))]
public class ColorEditor : Editor
{
    // This function displays the params and we override it.
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // This class will have variable target that is type Object that points to what we want to change
        CharacterColorChanger colorChanger = (CharacterColorChanger)target;

        // Multiple classes we can use: EditorGUILayout, or GUILayout, etc/
        EditorGUILayout.LabelField("Press the button below to change the characters color!");
        if (GUILayout.Button("Generate Color"))
        {
            colorChanger.GenerateColor();
        }
        GUILayout.Box("Press the button below to reset all changes");
        if (GUILayout.Button("Reset Color"))
        {
            colorChanger.Reset();
        }
    }

}
