using UnityEngine;

public enum Difficult
{ 
    Easy,
    Medium,
    Hard
}

public class MiniGame : MonoBehaviour
{
    [Header("Main Parameters")]
    [SerializeField] protected string _name;
    [SerializeField] protected Difficult _difficult;
    [SerializeField] protected InteractionObject _interactionObject;

    [Header("Staged for perk system")]
    [SerializeField] protected int _requiredPerkLevel;

    public virtual void Initialize(InteractionObject interactionObject)
    {
        _interactionObject = interactionObject;
    }

    public virtual void ClearMiniGame()
    { 
    
    }

    public virtual void Success()
    { 
    
    }

    public virtual void Failed()
    { 
    
    }
}
