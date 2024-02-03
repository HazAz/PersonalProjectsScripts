using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public class MenuItem : MonoBehaviour
{
    /// <summary>
    /// Creates menu item at the top of unity
    /// </summary>
    [UnityEditor.MenuItem("Custom Editor/Enable Gizmos")]
    private static void EnableGizmos()
    {
        // to toggle ON gizmos:
        SceneView.lastActiveSceneView.drawGizmos = true;
    }

    [UnityEditor.MenuItem("Custom Editor/Disable Gizmos")]
    private static void DisableGizmos()
    {
        // to toggle OFF gizmos:
        SceneView.lastActiveSceneView.drawGizmos = false;
    }
}
