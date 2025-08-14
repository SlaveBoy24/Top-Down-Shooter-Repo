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

        ExitGames.Client.Photon.Hashtable playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        if (!playerProperties.ContainsKey("Healths") || !playerProperties.ContainsKey("MaxHealths"))
            UpdatePhotonPlayerProperties();
    }

    public void AddHealths(float value)
    {
        _healths += value;

        if (_healths >= _maxHealths)
            _healths = _maxHealths;

        OnChangeHealthsAction?.Invoke();
        UpdatePhotonPlayerProperties();
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
        UpdatePhotonPlayerProperties();
    }

    public (float, float) GetHealthsData()
    {
        return (_maxHealths, _healths);
    }

    public void UpdatePhotonPlayerProperties()
    {
        ExitGames.Client.Photon.Hashtable playerCustomProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        if (playerCustomProperties.ContainsKey("MaxHealths"))
            playerCustomProperties["MaxHealths"] = _maxHealths;
        else
            playerCustomProperties.Add("MaxHealths", _maxHealths);

        if (playerCustomProperties.ContainsKey("Healths"))
            playerCustomProperties["Healths"] = _healths;
        else
            playerCustomProperties.Add("Healths", _healths);

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
    }
}
