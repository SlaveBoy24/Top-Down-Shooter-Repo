using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public Animator Animator;
    public FullBodyBipedIK FBBIK;
    public List<WeaponBase> EquipedWeapons = new List<WeaponBase>();
    public void EquipWeapon(WeaponBase weapon)
    {
        Spawn(weapon);
    }

    public void DeEquipWeapon(string name)
    {
        foreach (WeaponBase weapon in EquipedWeapons)
        {
            if (weapon.stashName == name)
            {
                Destroy(weapon.weaponMesh);
                EquipedWeapons.Remove(weapon);
                //UpdatePlayerProperties();

                if (EquipedWeapons.Count == 0)
                {
                    Animator.SetTrigger("unequip");
                }
                else
                {
                    switch (EquipedWeapons[0].mainItemScriptable.Type)
                    {
                        case ItemType.WeaponMain:
                            Animator.SetTrigger("rifle");
                            break;
                        case ItemType.WeaponSecondary:
                            Animator.SetTrigger("pistol");
                            break;
                    }
                    FBBIK.solver.leftHandEffector.target = EquipedWeapons[0].weaponObjects.LeftArmPoint.transform;
                    FBBIK.solver.leftArmChain.bendConstraint.bendGoal = EquipedWeapons[0].weaponObjects.BendGoalPoint.transform;
                    
                    return;
                }

                StartCoroutine(ChangeValueSmoothly(1, 0, 0.3f));

                return;
            }
        }
    }

    private void Spawn(WeaponBase weapon)
    {
        foreach (WeaponBase weaponBase in EquipedWeapons)
        {
            if (weapon.mainItemScriptable.Type == weaponBase.mainItemScriptable.Type)
            {
                return;    
            }
        }

        GameObject weaponObject = Instantiate(weapon.mainItemScriptable.GunSettings.Prefab);

        weaponObject.name = weapon.mainItemScriptable.name;

        Transform rightHandTransform = Animator.GetBoneTransform(HumanBodyBones.RightHand);

        weaponObject.transform.SetParent(rightHandTransform);

        weapon.weaponMesh = weaponObject;

        EquipedWeapons.Add(weapon);

        weaponObject.transform.localPosition = weapon.mainItemScriptable.GunSettings.RightHandPosition;
        weaponObject.transform.localEulerAngles = weapon.mainItemScriptable.GunSettings.RightHandRotation;
        weaponObject.transform.localScale = weapon.mainItemScriptable.GunSettings.RightHandScale;

        //weaponObject.layer = LayerMask.NameToLayer("Character");
        //не забывать ставить лаер чарактер

        if (weapon.mainItemScriptable.Type == ItemType.WeaponMain)
            Animator.SetTrigger("rifle");
        else
            Animator.SetTrigger("pistol");

        weapon.weaponObjects = weaponObject.GetComponent<WeaponObjects>();

        FBBIK.solver.leftHandEffector.target = weapon.weaponObjects.LeftArmPoint.transform;
        FBBIK.solver.leftArmChain.bendConstraint.bendGoal = weapon.weaponObjects.BendGoalPoint.transform;

        StartCoroutine(ChangeValueSmoothly(0, 1, 0.3f));
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
}