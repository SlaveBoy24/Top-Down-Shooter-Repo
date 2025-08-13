using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float _maxHealths;
    [SerializeField] private float _healths;

    public Action OnChangeHealthsAction;

    public void Initialize()
    {
        // to do save load
    }

    public void AddHealths(float value)
    {
        _healths += value;

        if (_healths >= _maxHealths)
            _healths = _maxHealths;

        OnChangeHealthsAction?.Invoke();
    }

    public void ConsumeHealths(float value)
    {
        _healths -= value;

        if (_healths <= 0)
        {
            _healths = 0;

            // to do die/nocked logic
        }

        OnChangeHealthsAction?.Invoke();
    }

    public (float, float) GetHealthsData()
    {
        return (_maxHealths, _healths);
    }
}
