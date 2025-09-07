using UnityEngine;

public class CharacterLook : MonoBehaviour
{
    [SerializeField] private GamePlayer _gamePlayer;
    [SerializeField] private bool _isLooking;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private FixedJoystick _joystick;
    
    private Animator _animator;
    public WeaponSystem WeaponSystem;

    private void Start()
    {
        if (!_gamePlayer.IsLocal())
        {
            Destroy(this);
        }

        _joystick = GameObject.FindGameObjectWithTag("PlayerLookJoystick").GetComponent<FixedJoystick>();
        _animator = GetComponent<Animator>();
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

            Vector3 animationDirection = transform.InverseTransformDirection(lookRot);

            _animator.SetFloat("Moving Ready X", animationDirection.x);
            _animator.SetFloat("Moving Ready Y", animationDirection.z);

            // нужна проверка на оружие
            _animator.SetBool("ready", true);

            WeaponSystem.SetAimIKWeight(1);
        }
        else
        {
            _isLooking = false;

            _animator.SetBool("ready", false);

            _animator.SetFloat("Moving Ready X", 0);
            _animator.SetFloat("Moving Ready Y", 0);

            WeaponSystem.SetAimIKWeight(0);
        }
    }

    public bool IsLooking()
    { 
        return _isLooking;
    }
}
