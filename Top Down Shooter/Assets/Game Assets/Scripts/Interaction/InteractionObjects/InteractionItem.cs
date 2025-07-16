using UnityEngine;

public class InteractionItem : InteractionObject
{
    private void Start()
    {
        _lockedStatus = InteractionLockedStatus.Unlocked;
    }

    public override void Interact()
    {

    }

    public override bool IsAbleToInteract()
    {
        return base.IsAbleToInteract(); // remove
    }
}
