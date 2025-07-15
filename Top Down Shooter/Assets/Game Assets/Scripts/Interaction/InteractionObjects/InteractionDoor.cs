using UnityEngine;

public class InteractionDoor : InteractionObject
{
    public override void Interact()
    {

    }

    public override bool IsAbleToInteract()
    {
        return base.IsAbleToInteract(); // remove
    }
}
