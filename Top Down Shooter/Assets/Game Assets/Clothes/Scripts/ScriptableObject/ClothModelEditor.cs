using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ClothModel)), CanEditMultipleObjects]
public class ClothModelEditor : Editor
{
    ClothModel targetSkript;
    GameObject player;
    GameObject clouth;

    public override void OnInspectorGUI()
    {
        targetSkript = (ClothModel)target;
        player = (GameObject)EditorGUILayout.ObjectField("Target Player", player, typeof(GameObject), true);
        clouth = (GameObject)EditorGUILayout.ObjectField("Target Clouth", clouth, typeof(GameObject), true);
        if (GUILayout.Button("Create"))
        {
            Create();
        }
        
        EditorGUILayout.Space();

        DrawDefaultInspector();
    }

    void Create()
    {

        Transform[] bonesPlayer = player.GetComponent<SkinnedMeshRenderer>().bones;
        Transform[] bonesClouth = clouth.GetComponent<SkinnedMeshRenderer>().bones;
        List<int> boneId = new List<int>();
        //List<int> bonesNotEqual = new List<int>();
        //Dictionary<int, Transform> bonesNotEqualD = new Dictionary<int, Transform>();
        for (int x = 0; x < bonesClouth.Length; x++)
        {
            for (int i = 0; i < bonesPlayer.Length; i++)
            {
                if (bonesPlayer[i].gameObject.name == bonesClouth[x].gameObject.name)
                {
                    boneId.Add(i);
                    //bonesNotEqual.Add(i, bonesPlayer[i]);
                    //Debug.Log(bonesPlayer[i].gameObject.name + " " + bonesClouth[x].gameObject.name);
                }
            }
        }
        targetSkript.bone.Clear();
        for (int i = 0; i < boneId.Count; i++)
        {
            targetSkript.bone.Add(boneId[i]);
        }

        // if (targetSkript.bonesNotEqual)
        // {
        //     for (int x = 0; x < bonesClouth.Length; x++)
        //     {
        //         for (int i = 0; i < bonesPlayer.Length; i++)
        //         {
        //             if (bonesPlayer[i].gameObject.name != bonesClouth[x].gameObject.name)
        //             {
        //                 if(!bonesNotEqualD.ContainsValue(bonesClouth[x]))
        //                 {
        //                     Debug.Log($"{x} {bonesClouth[x].name}");
        //                     bonesNotEqualD.Add(x, bonesClouth[x]);
        //                 }
        //             }
        //         }
        //     }

        //     foreach (var bonePlayer in bonesPlayer)
        //     {
        //         foreach (var bone in bonesNotEqualD)
        //         {
        //             if(bone.Value.name == bonePlayer.name)
        //             {
        //                 bonesNotEqual.Add(bone.Key);
        //             }
        //         }
        //     }

        //     foreach (int boneIdToDelete in bonesNotEqual)
        //     {
        //         bonesNotEqualD.Remove(boneIdToDelete);
        //     }
        //     targetSkript.additionBones.Clear();
        //     foreach (var bones in bonesNotEqualD)
        //     {
        //         Debug.Log($"{bones.Key} {bones.Value.name}");
        //         targetSkript.additionBones.Add(bones.Value);
        //     }
        // }


        boneId.Clear();
        // bonesNotEqual.Clear();
        // bonesNotEqualD.Clear();
        EditorUtility.SetDirty(targetSkript);
        AssetDatabase.SaveAssets();
    }

}
