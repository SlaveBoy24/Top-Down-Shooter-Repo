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
        IsLocked = false;
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

    public override void Initialize(InteractionObject interactionObject)
    {
        base.Initialize(interactionObject);

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
                SetupLockerSticks(Random.Range(3, 5), 2f);
                break;
            case Difficult.Medium:
                SetupLockerSticks(Random.Range(3, 6), 1.6f);
                break;
            case Difficult.Hard:
                SetupLockerSticks(Random.Range(4, 8), 1.2f);
                break;
        }
    }

    private void SetupLockerSticks(int amount, float diffScaleX)
    {
        for (int i = 0; i < amount; i++)
        { 
            LockerStick stick = new LockerStick();

            stick.Object = Instantiate(_lockerStickPrefab, _lockerSticksSpawnPoint.transform);
            stick.Object.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            stick.Object.transform.localScale = new Vector3(diffScaleX, 1, 1);
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

    public void TryUnlockStick()
    {
        float picklockRotationZ = _picklock.transform.localRotation.eulerAngles.z;
        float unlockRange = GetStickUnlockRange();
        foreach (LockerStick stick in _lockerSticks)
        {
            if (!stick.IsLocked)
                continue;

            float stickRotationZ = stick.Object.transform.localRotation.eulerAngles.z;
            float minZ = stickRotationZ - unlockRange;
            float maxZ = stickRotationZ + unlockRange;

            Debug.Log($"stick {stickRotationZ} - picklock {picklockRotationZ}, min {minZ}, max {maxZ}");

            if (picklockRotationZ <= maxZ && picklockRotationZ >= minZ)
            {
                stick.Unlock();
                CheckMinigame();
                return;
            }
            else
            {
                Debug.Log("Чек некст");
            }
        }
    }

    private float GetStickUnlockRange()
    {
        switch (_difficult)
        {
            case Difficult.Easy:
                return 5f;
            case Difficult.Medium:
                return 3.5f;
            case Difficult.Hard:
                return 2.75f;
            default:
                return 5f;
        }
    }

    private void CheckMinigame()
    {
        bool isUnlocked = true;

        foreach (LockerStick stick in _lockerSticks)
        {
            if (stick.IsLocked)
            {
                isUnlocked = false;
                break;
            }
        }

        if (isUnlocked)
        {
            Success();
        }
    }

    public override void Success()
    {
        ClearMiniGame();
        _interactionObject.Unlock();
        Interactor.Instance.UpdateUI();
    }

    public override void Failed()
    {
        ClearMiniGame();
    }
}
