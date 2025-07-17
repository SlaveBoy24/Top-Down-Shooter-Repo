using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LockBreaking : MIniGame
{
    [Header("Required Objects")]
    [SerializeField] private GameObject _pickLock;
    [SerializeField] private GameObject _lockerStickPrefab;

    [Header("Logic")]
    [SerializeField] private List<GameObject> _lockerSticks;
    [SerializeField] private List<Image> _lockerStickImages;
    [SerializeField] private bool _isSpin;
    [SerializeField] private int _rotationSpeed;

    public override void Initialize()
    {
        switch (_difficult)
        {
            case Difficult.Easy:
                break;
            case Difficult.Medium:
                break;
            case Difficult.Hard:
                break;
        }
    }

    public void StartGame()
    { 
    
    }

    public void TryPickLock()
    { 
    
    }

    public override void Success()
    {

    }

    public override void Failed()
    {

    }
}
