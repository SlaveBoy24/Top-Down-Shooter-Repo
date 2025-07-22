using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject WeaponHandler;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
}
