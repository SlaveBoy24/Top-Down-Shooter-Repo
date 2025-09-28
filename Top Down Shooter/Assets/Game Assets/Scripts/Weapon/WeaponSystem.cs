using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WeaponSystem : MonoBehaviour
{
    public Animator Animator;
    public FullBodyBipedIK FBBIK;
    public AimIK AimIK;
    public List<WeaponBase> EquipedWeapons = new List<WeaponBase>();
    private MainPlayer _mainPlayer;

    public void EquipWeapon(WeaponBase weapon)
    {
        Spawn(weapon);
    }
    public void BoolTrigger(string s)
    {
        switch (s)
        {
            case "unequip":
                Animator.SetBool("unequip", true);
                Animator.SetBool("rifle", false);
                Animator.SetBool("pistol", false);
                break;
            case "rifle":
                Animator.SetBool("unequip", false);
                Animator.SetBool("rifle", true);
                Animator.SetBool("pistol", false);
                break;
            case "pistol":
                Animator.SetBool("unequip", false);
                Animator.SetBool("rifle", false);
                Animator.SetBool("pistol", true);
                break;
        }
    }
    public void DeEquipAllWeapon()
    {
        foreach (WeaponBase weapon in EquipedWeapons)
        {
            Destroy(weapon.weaponMesh);
            EquipedWeapons.Remove(weapon);

            DeEquipCheck();

            StartCoroutine(ChangeValueSmoothly(1, 0, 0.3f));

            break;
        }
    }
    public void DeEquipWeapon(string name)
    {
        foreach (WeaponBase weapon in EquipedWeapons)
        {
            if (weapon.stashName == name)
            {
                Destroy(weapon.weaponMesh);
                EquipedWeapons.Remove(weapon);
                UpdatePlayerProperties();

                DeEquipCheck();

                StartCoroutine(ChangeValueSmoothly(1, 0, 0.3f));

                break;
            }
        }

        // when u despawn main gun he got a second
        if ((GlobalStashes.MainWeaponSlot.Items[0] == null) && (name == "Weapon Main"))
        {
            if (GlobalStashes.SecondaryWeaponSlot.Items[0] != null)
            {
                Spawn(new WeaponBase(GlobalStashes.SecondaryWeaponSlot));
            }
        } 
    }
    private void DeEquipCheck()
    {
        if (EquipedWeapons.Count == 0)
        {
            BoolTrigger("unequip");
        }
        else
        {
            switch (EquipedWeapons[0].mainItemScriptable.Type)
            {
                case ItemType.WeaponMain:
                    BoolTrigger("rifle");
                    break;
                case ItemType.WeaponSecondary:
                    BoolTrigger("pistol");
                    break;
            }
            FBBIK.solver.leftHandEffector.target = EquipedWeapons[0].weaponObjects.LeftArmPoint.transform;
            FBBIK.solver.leftArmChain.bendConstraint.bendGoal = EquipedWeapons[0].weaponObjects.BendGoalPoint.transform;
        }
    }
    public void SetAimIKWeight(float w)
    {
        if (EquipedWeapons.Count == 0)
        {
            AimIK.solver.IKPositionWeight = 0;
            return;
        }

        AimIK.solver.IKPositionWeight = w;
    }

    [Obsolete]
    private void Start()
    {
        _mainPlayer = FindObjectOfType<MainPlayer>();
    }

    public void ChangeWeapon(WeaponBase weapon = null)
    {
        if (weapon == null)
        {
            DeEquipAllWeapon();
        }
        else
        {
            
        }
    }

    #region Spawn Weapon
    private void Spawn(WeaponBase weapon, bool updateProps = true)
    {
        foreach (WeaponBase weaponBase in EquipedWeapons)
        {
            if (weapon.mainItemScriptable.Type == weaponBase.mainItemScriptable.Type)
            {
                return;
            }

            if ((weapon.mainItemScriptable.Type == ItemType.WeaponSecondary) && (weaponBase.mainItemScriptable.Type == ItemType.WeaponMain))
            {
                return;
            }

            if ((weapon.mainItemScriptable.Type == ItemType.WeaponMain) && (weaponBase.mainItemScriptable.Type == ItemType.WeaponSecondary))
            {
                DeEquipWeapon("Weapon Second");
                break;
            }
        }

        InstantiateAndSetWeapon(weapon);

        //weaponObject.layer = LayerMask.NameToLayer("Character");
        //не забывать ставить лаер чарактер

        SetAnimator(weapon);
        SetIK(weapon);

        StartCoroutine(ChangeValueSmoothly(0, 1, 0.4f));

        if (updateProps)
            UpdatePlayerProperties();
    }

    private void InstantiateAndSetWeapon(WeaponBase weapon)
    {
        GameObject weaponObject = Instantiate(weapon.mainItemScriptable.GunSettings.Prefab);

        weaponObject.name = weapon.mainItemScriptable.name;

        Transform rightHandTransform = Animator.GetBoneTransform(HumanBodyBones.RightHand);

        weaponObject.transform.SetParent(rightHandTransform);

        weapon.weaponMesh = weaponObject;

        EquipedWeapons.Add(weapon);

        weaponObject.transform.localPosition = weapon.mainItemScriptable.GunSettings.RightHandPosition;
        weaponObject.transform.localEulerAngles = weapon.mainItemScriptable.GunSettings.RightHandRotation;
        weaponObject.transform.localScale = weapon.mainItemScriptable.GunSettings.RightHandScale;

        weapon.weaponObjects = weaponObject.GetComponent<WeaponObjects>();
    }

    private void SetAnimator(WeaponBase weapon)
    {
        Debug.Log("SET TRIGGER METHOD");
        if (weapon.mainItemScriptable.Type == ItemType.WeaponMain)
        {
            Debug.Log("SET TRIGGER rifle");
            BoolTrigger("rifle");
        }
        else
        {
            Debug.Log("SET TRIGGER pistol");
            BoolTrigger("pistol");
        }
    }

    private void SetIK(WeaponBase weapon)
    {
        FBBIK.solver.leftHandEffector.target = weapon.weaponObjects.LeftArmPoint.transform;
        FBBIK.solver.leftArmChain.bendConstraint.bendGoal = weapon.weaponObjects.BendGoalPoint.transform;

        //NOT GAME SCENE
        try
        {
            AimIK.solver.transform = weapon.weaponObjects.FireTagPoint.transform;
        }
        catch{}
    }

    public IEnumerator ChangeValueSmoothly(float startValue, float targetValue, float duration)
    {
        float elapsedTime = 0f;
        FBBIK.solver.leftHandEffector.positionWeight = startValue;
        FBBIK.solver.leftHandEffector.rotationWeight = startValue;
        FBBIK.solver.leftArmChain.bendConstraint.weight = startValue;

        while (elapsedTime < duration)
        {
            var currentValue = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);

            FBBIK.solver.leftHandEffector.positionWeight = currentValue;
            FBBIK.solver.leftHandEffector.rotationWeight = currentValue;
            FBBIK.solver.leftArmChain.bendConstraint.weight = currentValue;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        FBBIK.solver.leftHandEffector.positionWeight = targetValue;
        FBBIK.solver.leftHandEffector.rotationWeight = targetValue;
        FBBIK.solver.leftArmChain.bendConstraint.weight = targetValue;
    }
    #endregion

    public void UpdatePlayerProperties()
    {
        string weaponKeys = "";
        foreach (WeaponBase weapon in EquipedWeapons)
            if (weapon.mainItemScriptable != null)
                weaponKeys += $"{weapon.mainItemScriptable.name.CamelToSnake()} ";

        if (weaponKeys != "")
            weaponKeys = weaponKeys.Substring(0, weaponKeys.Length - 1);

        ExitGames.Client.Photon.Hashtable playerCustomProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        if (!playerCustomProperties.ContainsKey("equipedWeapon"))
            playerCustomProperties.Add("equipedWeapon", weaponKeys);
        else
            playerCustomProperties["equipedWeapon"] = weaponKeys;

        Debug.Log("weaponKeys(" + weaponKeys + ")");

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
    }

    public void OnUpdatePlayerProperties(Player player)
    {
        DeEquipAllWeapon();

        ExitGames.Client.Photon.Hashtable playerCustomProperties = player.CustomProperties;
        if (!playerCustomProperties.ContainsKey("equipedWeapon"))
            return;

        string weaponKeys = (string)playerCustomProperties["equipedWeapon"];

        Debug.Log(weaponKeys);

        foreach (string key in weaponKeys.Split(" "))
        {
            ItemScriptableObject item = null;
            ItemPool.All.TryGetValue(key, out item);

            if (item != null)
                Spawn(new WeaponBase(item), false);
        }
    }
}

[System.Serializable]
public class WeaponBase
{
    public string stashName;
    public ItemScriptableObject mainItemScriptable;
    public GameObject weaponMesh;
    public WeaponObjects weaponObjects;
    public WeaponBase(Stash stash)
    {
        this.mainItemScriptable = stash.Items[0].item;
        this.stashName = stash.transform.gameObject.name;
    }

    public WeaponBase(ItemScriptableObject item)
    {
        this.mainItemScriptable = item;
    }
}
