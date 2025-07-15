using UnityEngine;

public enum InteractionType
{ 
    Item,
    Stash,
    Door,
    Terminal
}

public class InteractionObject : MonoBehaviour
{
    [SerializeField] private InteractionType _type;

    public InteractionType GetInteractionType()
    { 
        return _type;
    }

    public virtual void Interact()
    { 
    
    }

    public virtual bool IsAbleToInteract()
    {
        return false;
    }
}
