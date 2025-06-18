using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private bool _pcControl;

    [Space]
    [SerializeField] private bool _isMoving;
    [SerializeField] private CharacterLook _characterLook;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float maxVelocityChange = 10f;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Transform _movementDirectionPoint;

    private Animator _animator;
    private Rigidbody _rb;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _joystick = GameObject.FindGameObjectWithTag("PlayerMoveJoystick").GetComponent<FixedJoystick>();
    }

    private void FixedUpdate()
    {
        float horizontal = 0;
        float vertical = 0;

        if (!_pcControl)
        {
            horizontal = _joystick.Horizontal;
            vertical = _joystick.Vertical;
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        Vector3 lookRot = new Vector3(horizontal, 0, vertical);
        lookRot.Normalize();

        float strengthOfMove = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        if (horizontal != 0 || vertical != 0)
        {
            _isMoving = true;
            if (!_characterLook.IsLooking())
            {
                Quaternion lookDir = Quaternion.LookRotation(lookRot);
                Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookDir, _rotationSpeed);
                transform.rotation = targetRot;
                _animator.SetFloat("Moving X", 0);
                _animator.SetFloat("Moving Y", strengthOfMove);
            }
            else
            {
                Vector3 animationDirection = transform.InverseTransformDirection(lookRot);

                _animator.SetFloat("Moving X", animationDirection.x);
                _animator.SetFloat("Moving Y", animationDirection.z);
            }
            Vector3 moveVelocity = CalculateMovement(lookRot, strengthOfMove);
            return;
            _rb.AddForce(moveVelocity, ForceMode.VelocityChange);
        }
        else
        {
            _isMoving = false;
            _animator.SetFloat("Moving X", 0);
            _animator.SetFloat("Moving Y", 0);
        }
    }

    private Vector3 CalculateMovement(Vector3 input, float strength)
    {
        Vector3 targetVelocity = input.x * _movementDirectionPoint.right + input.z * _movementDirectionPoint.forward;
        targetVelocity = _movementDirectionPoint.TransformDirection(targetVelocity);
        targetVelocity *= _movementSpeed * strength;

        Vector3 velocity = _rb.linearVelocity;
        if (input.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            return velocityChange;
        }
        else
        {
            return new Vector3();
        }
    }

    public bool IsMoving()
    {
        return _isMoving;
    }
}