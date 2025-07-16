using UnityEngine;

public class InteractionDoor : InteractionObject
{
    [SerializeField] private bool _isOpened;
    [SerializeField] private bool _isLocked;
    [SerializeField] private int _requiredPerkLevel;
    private Animation _animation;

    private void Start()
    {
        _animation = GetComponent<Animation>();
    }

    public override void Interact()
    {
        if (_isLocked)
        {
            // TODO: create mini game
        }
        else
        {
            _isOpened = true;
            _animation.Play();
        }
    }

    public override bool IsAbleToInteract()
    {
        if (!_isOpened)
        {
            if (_isLocked)
            {
                // TODO: create mini game
                return false;
            }
            else
            {
                return true;
            }
        }

        return false;
    }
}
