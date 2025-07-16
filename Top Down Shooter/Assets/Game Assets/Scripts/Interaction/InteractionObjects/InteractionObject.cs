using UnityEngine;

public enum InteractionType
{ 
    Item,
    Stash,
    Door,
    Terminal
}

public enum InteractionLockedStatus
{ 
    Unlocked,
    Locked,
    LockedByPerk
}

public class InteractionObject : MonoBehaviour
{
    [SerializeField] private InteractionType _type;
    [SerializeField] protected InteractionLockedStatus _lockedStatus;

    public InteractionType GetInteractionType()
    { 
        return _type;
    }

    public InteractionLockedStatus GetInteractionLockedStatus()
    {
        return _lockedStatus;
    }

    public virtual void Interact()
    { 
    
    }

    public virtual bool IsAbleToInteract()
    {
        return false;
    }
}
