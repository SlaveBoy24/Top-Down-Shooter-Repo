using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject WeaponHandler;
    private Animator _animator;
    public Item InHandWeapon;
    public GameObject ShootPrefab;
    public Vector3 testOffset;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Shoot()
    {
        StartCoroutine(OneShoot());
    }
    private IEnumerator OneShoot()
    {
        float bulletSpeed = 40f;

        _animator.SetTrigger("shoot");

        var bullet = Instantiate(ShootPrefab);

        bullet.transform.position = this.transform.position + testOffset; // for offset

        bullet.transform.LookAt(this.gameObject.transform.position + testOffset + transform.forward);

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        bulletRigidbody.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.VelocityChange);

        yield return new WaitForSeconds(1.5f);

        Destroy(bullet);

        yield break;
    }
    public void ChangeState(string state)
    {
        switch (state)
        {
            case "rifle":
                _animator.SetTrigger("rifle");
                break;
            case "pistol":
                _animator.SetTrigger("pistol");
                break;
            default:
                _animator.SetTrigger("unequip");
                break;
        }
    }
}
