using System.Collections;
using UnityEngine;
using Photon.Pun;

public class WeaponController : MonoBehaviour
{
    private WeaponSystem _weaponSystem;
    public GameObject ShootPrefab;
    public GameObject GunshotPrefab;
    public float BulletSpeed;
    private void Start()
    {
        _weaponSystem = GetComponent<WeaponSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed();
        }
    }

    void OnSpacePressed()
    {
        if (_weaponSystem.EquipedWeapons.Count == 0)
            return;

        if (_weaponSystem.Animator.GetBool("ready") == false)
            return;

        StartCoroutine(OneShoot());
    }

    private IEnumerator OneShoot()
    {
        _weaponSystem.Animator.SetTrigger("shoot");

        WeaponBase weapon = _weaponSystem.EquipedWeapons[0];

        Vector3 position = weapon.weaponObjects.FireTagPoint.transform.position;
        Quaternion rotation = weapon.weaponObjects.FireTagPoint.transform.rotation;

        GameObject bullet = PhotonNetwork.Instantiate(ShootPrefab.name, position, rotation);

        GameObject gunShot = PhotonNetwork.Instantiate(GunshotPrefab.name, position, rotation);

        yield return new WaitUntil(() => bullet != null);

        BulletController bulletController = bullet.GetComponent<BulletController>();

        bulletController.SetDamage(weapon.mainItemScriptable.DamageDeal);

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        bulletRigidbody.AddForce(bullet.transform.forward * BulletSpeed, ForceMode.VelocityChange);

        yield return new WaitForSeconds(1f);

        PhotonNetwork.Destroy(gunShot);

        yield break;
    }

    public void ChangeState(string state)
    {
        switch (state)
        {
            case "rifle":
                _weaponSystem.Animator.SetTrigger("rifle");
                break;
            case "pistol":
                _weaponSystem.Animator.SetTrigger("pistol");
                break;
            default:
                _weaponSystem.Animator.SetTrigger("unequip");
                break;
        }
    }
}
