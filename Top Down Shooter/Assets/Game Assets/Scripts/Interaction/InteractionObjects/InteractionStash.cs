using UnityEngine;

public class InteractionStash : InteractionObject
{
    public override void Interact()
    {

    }

    public override bool IsAbleToInteract()
    {
        return base.IsAbleToInteract(); // remove
    }
}
