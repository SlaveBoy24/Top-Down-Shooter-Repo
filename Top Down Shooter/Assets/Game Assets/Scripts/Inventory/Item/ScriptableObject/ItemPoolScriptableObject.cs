using System;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ItemPoolScriptableObject))]
public class ItemPoolScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemPoolScriptableObject targetComponent = (ItemPoolScriptableObject)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUI.indentLevel++;


        if (GUILayout.Button("Grab All Items"))
        {
            targetComponent.ItemsPoolList.Clear();
            var guids = AssetDatabase.FindAssets("t:ItemScriptableObject");

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);

                var item = AssetDatabase.LoadAssetAtPath<ItemScriptableObject>(path);

                targetComponent.ItemsPoolList.Add(new LikeDict(item.name.CamelToSnake(), item));
            }

            serializedObject.ApplyModifiedProperties();
        }


        EditorGUI.indentLevel--;
    }
}
#endif



[CreateAssetMenu(fileName = "ItemPoolScriptableObject", menuName = "ItemPoolScriptableObject", order = 0)]
public class ItemPoolScriptableObject : ScriptableObject
{
    private static ItemPoolScriptableObject _instance;
    public static ItemPoolScriptableObject instance
    {
        get
        {
            if (!_instance)
                _instance = Resources.Load("ItemPoolScriptableObject") as ItemPoolScriptableObject;
            if (!_instance)
                _instance = CreateInstance<ItemPoolScriptableObject>();
            return _instance;
        }
    }

    [SerializeField] public List<LikeDict> ItemsPoolList;
}

[Serializable]
public class LikeDict
{
    public string key;
    public ItemScriptableObject value;
    public LikeDict(string k, ItemScriptableObject v)
    {
        key = k;
        value = v;
    }
}
