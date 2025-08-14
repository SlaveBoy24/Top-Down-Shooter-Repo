using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;

/// <summary> Sets a background color for game objects in the Hierarchy tab</summary>
[UnityEditor.InitializeOnLoad]
#endif
public class CustomHierarchy
{
    private static Vector2 offset = new Vector2(20, 0);
    const string IgnoreIcons = "GameObject Icon, Prefab Icon, d_GameObject Icon, d_Prefab Icon";

    static CustomHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {

        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj != null)
        {
            Color backgroundColor = new Color(0.219f, 0.219f, 0.219f);
            Color textColor = Color.white;
            //Texture2D texture = null;      
           
            var content = EditorGUIUtility.ObjectContent(EditorUtility.InstanceIDToObject(instanceID), null);

            if (content.image != null && !IgnoreIcons.Contains(content.image.name))
                {
                    EditorGUI.DrawRect(new Rect(selectionRect.position, new Vector2(selectionRect.height, selectionRect.height)), backgroundColor);
                    GUI.DrawTexture(new Rect(selectionRect.position, new Vector2(selectionRect.height, selectionRect.height)), content.image);                       
                       
                }
        }
    }
}