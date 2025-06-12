using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private DynamicJoystick _joystick;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<DynamicJoystick>();
    }

    private void FixedUpdate()
    {
        float horizontal = _joystick.Horizontal;
        float vertical = _joystick.Vertical;

        Vector3 lookRot = new Vector3(horizontal, 0, vertical);
        lookRot.Normalize();

        float setMove = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        _animator.SetFloat("Moving", setMove);

        if (horizontal != 0 || vertical != 0)
        {
            Quaternion lookDir = Quaternion.LookRotation(lookRot);
            Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookDir, _rotationSpeed);
            transform.rotation = targetRot;
        }
    }
}