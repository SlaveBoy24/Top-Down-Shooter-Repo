using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ClothSystem)), CanEditMultipleObjects]
public class ClothSystemEditor : Editor
{
    ClothSystem targetScript;
    ClothModel cloth;
    public override void OnInspectorGUI()
    {
        targetScript = (ClothSystem)target;
        
        DrawDefaultInspector();
        EditorGUILayout.Space();

        cloth = EditorGUILayout.ObjectField("ScriptableObject Cloth",cloth,typeof(ClothModel),true) as ClothModel;

        if (GUILayout.Button("Equip"))
        {
            Equip(cloth);
        }
    }
    public void Equip(ClothModel cloth)
    {
        targetScript.EquipCloth(new ClothBase(cloth));
    }
}
#endif