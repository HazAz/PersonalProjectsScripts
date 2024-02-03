using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

[CustomEditor(typeof(GameData))]
public class GameDataObjectCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor"))
        {
            GameDataObjectEditorWindow.Open((GameData)target);
        }
    }
}

/// <summary>
/// Double click asset to open. Needs to include UnityEditor.Callbacks
/// </summary>
public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        GameData obj = EditorUtility.InstanceIDToObject(instanceId) as GameData;
        if (obj != null)
        {
            GameDataObjectEditorWindow.Open((obj));
            return true;
        }
        return false;
    }
}
