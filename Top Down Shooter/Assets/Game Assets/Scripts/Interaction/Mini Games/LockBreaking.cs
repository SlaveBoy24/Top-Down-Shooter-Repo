using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LockerStick
{
    public GameObject Object;
    public Image Icon;
    public bool IsLocked = true;

    public void Unlock()
    {
        Object.transform.localScale = new Vector3(1, 0.8f, 1);
        Icon.color = new Color(0, 0, 0, 0.5f);
    }
}

public class LockBreaking : MiniGame
{
    [Header("Required Objects")]
    [SerializeField] private GameObject _picklock;
    [SerializeField] private GameObject _lockerSticksSpawnPoint;
    [SerializeField] private GameObject _lockerStickPrefab;

    [Header("Logic")]
    [SerializeField] private int _picklockHealts;
    [SerializeField] private List<LockerStick> _lockerSticks;
    [SerializeField] private bool _isPlaying;
    [SerializeField] private int _rotationSpeed;

    public override void Initialize()
    {
        ClearMiniGame();
        SetupDifficult();

        gameObject.SetActive(true);
        _isPlaying = true;

        StartCoroutine(PicklockSpinning());
    }

    public void QuitMiniGame()
    {
        ClearMiniGame();
    }

    public override void ClearMiniGame()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);

        foreach (var item in _lockerSticks)
            Destroy(item.Object);

        _lockerSticks.Clear();

        _picklockHealts = 5;
        _isPlaying = false;

        _picklock.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void SetupDifficult()
    {
        switch (_difficult)
        {
            case Difficult.Easy:
                SetupLockerSticks(Random.Range(3, 5));
                break;
            case Difficult.Medium:
                break;
            case Difficult.Hard:
                break;
        }
    }

    private void SetupLockerSticks(int amount)
    {
        for (int i = 0; i < amount; i++)
        { 
            LockerStick stick = new LockerStick();

            stick.Object = Instantiate(_lockerStickPrefab, _lockerSticksSpawnPoint.transform);
            stick.Object.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            stick.Icon = stick.Object.transform.GetChild(0).GetComponent<Image>();

            _lockerSticks.Add(stick);
        }
    }

    private IEnumerator PicklockSpinning()
    {
        while (_isPlaying)
        {
            _picklock.transform.rotation *= Quaternion.Euler(0, 0, _rotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return null;
    }

    public void TryPickLock()
    { 
    
    }

    public override void Success()
    {
        ClearMiniGame();
    }

    public override void Failed()
    {
        ClearMiniGame();
    }
}
