using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClothSystem : MonoBehaviour
{
    public SkinnedMeshRenderer original;
    private Transform[] boneOriginal;
    public List<ClothBase> EquipedClothes = new List<ClothBase>();
    private void Awake()
    {
        boneOriginal = original.bones;
    }
    public void EquipCloth(ClothBase clothBase)
    {
        Spawn(clothBase);
    }
    public void DeEquipCloth(string stashName)
    {
        foreach (ClothBase clothBase in EquipedClothes)
        {
            if (clothBase.stashName == stashName)
            {
                Destroy(clothBase.clothMesh);
                EquipedClothes.Remove(clothBase);
                return;
            }
        }
    }
    private void Spawn(ClothBase clothBase)
    {
        GameObject clothMesh = new GameObject(clothBase.clothTable.objectName);
        clothMesh.transform.SetParent(this.transform);

        clothMesh.transform.localPosition = Vector3.zero;
        clothMesh.transform.localEulerAngles = new Vector3(0, 0, 0);
        clothMesh.transform.localScale = new Vector3(100, 100, 100);

        SkinnedMeshRenderer skin = clothMesh.AddComponent<SkinnedMeshRenderer>();

        Transform[] bone = new Transform[clothBase.clothTable.bone.Count];
        skin.bones = new Transform[clothBase.clothTable.bone.Count];

        for (int x = 0; x < clothBase.clothTable.bone.Count; x++)
        {
            bone[x] = boneOriginal[clothBase.clothTable.bone[x]];
        }

        skin.sharedMesh = clothBase.clothTable.objectMesh;

        skin.bones = bone;

        skin.rootBone = original.rootBone;

        skin.materials = clothBase.clothTable.materials;

        clothBase.clothMesh = clothMesh;

        clothMesh.layer = LayerMask.NameToLayer("Character");

        EquipedClothes.Add(clothBase);
    }
}

[System.Serializable]
public class ClothBase
{
    public string stashName;
    public ClothModel clothTable;
    [HideInInspector]public GameObject clothMesh;
    public ClothBase(Stash stash)
    {
        this.clothTable = stash.Items[0].item.ClothScriptableObject;
        this.stashName = stash.transform.gameObject.name;
    }
    public ClothBase(ClothModel clothTable)
    {
        this.clothTable = clothTable;
    }
}