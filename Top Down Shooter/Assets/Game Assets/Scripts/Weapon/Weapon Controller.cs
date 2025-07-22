using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject WeaponHandler;
    private Animator _animator;
    public Item InHandWeapon;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Shoot()
    {
        _animator.SetTrigger("shoot");
        
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
