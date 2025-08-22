using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSctiptableObject", menuName = "Game Scriptable Objects/WeaponSctiptableObject")]
[Serializable]
public class WeaponScriptableObject : ScriptableObject
{
    public GameObject Prefab;
    public Vector3 RightHandPosition;
    public Vector3 RightHandRotation;
    public Vector3 RightHandScale;
}
