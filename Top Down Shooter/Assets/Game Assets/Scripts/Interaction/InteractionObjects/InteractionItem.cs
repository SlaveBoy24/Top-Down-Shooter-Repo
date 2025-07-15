using UnityEngine;

public class InteractionItem : InteractionObject
{
    public override void Interact()
    {

    }

    public override bool IsAbleToInteract()
    {
        return base.IsAbleToInteract(); // remove
    }
}
