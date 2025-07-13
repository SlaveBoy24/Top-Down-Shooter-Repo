#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GlobalInventory))]
public class GlobalInventoryEditor : Editor
{
    private ItemScriptableObject ItemScriptableObjectField;
    private string key;
    public override void OnInspectorGUI()
    {
        GlobalInventory targetComponent = (GlobalInventory)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUI.indentLevel++;

        ItemScriptableObjectField = (ItemScriptableObject)EditorGUILayout.ObjectField(
                "Item To Spawn",
                ItemScriptableObjectField,
                typeof(ItemScriptableObject),
                false);
            
        key = EditorGUILayout.TextField("Item To Spawn By Name", key);

        serializedObject.ApplyModifiedProperties();

        if ((ItemScriptableObjectField != null) || (key != ""))
        {
            if (ItemScriptableObjectField != null)
                EditorGUILayout.HelpBox($"Loaded: {ItemScriptableObjectField.name}", MessageType.Info);

            if (GUILayout.Button("Spawn"))
            {
                if (ItemScriptableObjectField == null)
                    GlobalStashes.Backpack.SpawnItemByKey(targetComponent.ItemPrefab, key);
                else
                    GlobalStashes.Backpack.SpawnItem(targetComponent.ItemPrefab, ItemScriptableObjectField);
            }
        }


        EditorGUI.indentLevel--;
    }
}
#endif