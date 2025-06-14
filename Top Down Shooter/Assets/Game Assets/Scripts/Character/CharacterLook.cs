using UnityEngine;

public class CharacterLook : MonoBehaviour
{
    [SerializeField] private bool _isLooking;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private FixedJoystick _joystick;

    private void Start()
    {
        _joystick = GameObject.FindGameObjectWithTag("PlayerLookJoystick").GetComponent<FixedJoystick>();
    }

    private void FixedUpdate()
    {
        float horizontal = _joystick.Horizontal;
        float vertical = _joystick.Vertical;

        Vector3 lookRot = new Vector3(horizontal, 0, vertical);
        lookRot.Normalize();

        if (horizontal != 0 || vertical != 0)
        {
            _isLooking = true;
            Quaternion lookDir = Quaternion.LookRotation(lookRot);
            Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookDir, _rotationSpeed);
            transform.rotation = targetRot;
        }
        else
        {
            _isLooking = false;
        }
    }

    public bool IsLooking()
    { 
        return _isLooking;
    }
}
