using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private WeaponSystem _weaponSystem;
    public GameObject ShootPrefab;
    public float BulletSpeed = 55f;
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

        GameObject bullet = Instantiate(ShootPrefab, _weaponSystem.EquipedWeapons[0].weaponObjects.FireTagPoint.transform.position, _weaponSystem.EquipedWeapons[0].weaponObjects.FireTagPoint.transform.rotation);

        yield return new WaitUntil(() => bullet != null);

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        bulletRigidbody.AddForce(bullet.transform.forward * BulletSpeed, ForceMode.VelocityChange);

        yield return new WaitForSeconds(1.5f);

        Destroy(bullet);

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
