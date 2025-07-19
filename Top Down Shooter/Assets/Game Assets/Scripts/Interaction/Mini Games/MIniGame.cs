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

    [Header("Staged for perk system")]
    [SerializeField] protected int _requiredPerkLevel;

    public virtual void Initialize()
    { 
    
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
