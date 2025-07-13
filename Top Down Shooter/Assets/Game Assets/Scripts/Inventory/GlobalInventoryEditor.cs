#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GlobalInventory))]
public class GlobalInventoryEditor : Editor
{
    private ItemSctiptableObject ItemScriptableObject;
    private string key;
    public override void OnInspectorGUI()
    {
        GlobalInventory targetComponent = (GlobalInventory)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUI.indentLevel++;

        ItemScriptableObject = (ItemSctiptableObject)EditorGUILayout.ObjectField(
                "Item To Spawn",
                ItemScriptableObject,
                typeof(ItemSctiptableObject),
                false);
            
        key = EditorGUILayout.TextField("Item To Spawn By Name", key);

        serializedObject.ApplyModifiedProperties();

        if ((ItemScriptableObject != null) || (key != ""))
        {
            if (ItemScriptableObject != null)
                EditorGUILayout.HelpBox($"Loaded: {ItemScriptableObject.name}", MessageType.Info);

            if (GUILayout.Button("Spawn"))
            {
                if (ItemScriptableObject == null)
                    GlobalStashes.Backpack.SpawnItemByKey(targetComponent.ItemPrefab, key);
                else
                    GlobalStashes.Backpack.SpawnItem(targetComponent.ItemPrefab, ItemScriptableObject);
            }
        }


        EditorGUI.indentLevel--;
    }
}
#endif