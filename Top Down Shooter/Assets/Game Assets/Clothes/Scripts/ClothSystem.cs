using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
                UpdatePlayerProperties();
                return;
            }
        }
    }

    private void DeEquipAll()
    {
        foreach (ClothBase clothBase in EquipedClothes)
        {
            Destroy(clothBase.clothMesh);
        }

        EquipedClothes = new List<ClothBase>();
    }

    private void Spawn(ClothBase clothBase, bool updateProps = true) // update props needs to sync players
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

        if (updateProps)
            UpdatePlayerProperties();
    }
    public void UpdatePlayerProperties()
    {
        string clothKeys = "";
        foreach (ClothBase cloth in EquipedClothes)
            if (cloth.mainItemScriptable != null)
                clothKeys += $"{cloth.mainItemScriptable.name.CamelToSnake()} ";

        if (clothKeys != "")
            clothKeys = clothKeys.Substring(0, clothKeys.Length - 1);

        ExitGames.Client.Photon.Hashtable playerCustomProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        if (!playerCustomProperties.ContainsKey("equipment"))
            playerCustomProperties.Add("equipment", clothKeys);
        else
            playerCustomProperties["equipment"] = clothKeys;

        Debug.Log("clothKeys(" + clothKeys + ")");

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
    }

    public void OnUpdatePlayerProperties(Player player)
    {
        DeEquipAll();

        ExitGames.Client.Photon.Hashtable playerCustomProperties = player.CustomProperties;
        if (!playerCustomProperties.ContainsKey("equipment"))
            return;

        string clothKeys = (string)playerCustomProperties["equipment"];

        Debug.Log(clothKeys);

        foreach (string key in clothKeys.Split(" "))
        {
            ItemScriptableObject item = null;
            ItemPool.All.TryGetValue(key, out item);

            if (item != null)
                Spawn(new ClothBase(item.ClothScriptableObject), false);
        }
    }
}


[System.Serializable]
public class ClothBase
{
    public string stashName;
    public ItemScriptableObject mainItemScriptable;
    public ClothModel clothTable;
    public GameObject clothMesh;
    public ClothBase(Stash stash)
    {
        this.mainItemScriptable = stash.Items[0].item;
        this.clothTable = stash.Items[0].item.ClothScriptableObject;
        this.stashName = stash.transform.gameObject.name;
    }
    public ClothBase(ClothModel clothTable)
    {
        this.clothTable = clothTable;
    }
}