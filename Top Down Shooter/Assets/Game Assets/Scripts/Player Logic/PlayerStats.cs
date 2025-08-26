using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float _maxHealths;
    [SerializeField] private float _healths;
    [SerializeField] private int _money;
    [SerializeField] private int _donateMoney;


    public Action OnChangeHealthsAction;
    public Action OnChangeMoneyAction;
    public Action OnChangeDonateMoneyAction;

    public void Initialize()
    {
        // to do save load

        ExitGames.Client.Photon.Hashtable playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        if (!playerProperties.ContainsKey("Healths") || !playerProperties.ContainsKey("MaxHealths"))
            UpdatePhotonPlayerProperties();
    }

    #region Healths
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
    #endregion

    #region Money
    public void AddMoney(int value)
    {
        _money += value;

        OnChangeMoneyAction?.Invoke();
    }

    public void ConsumeMoney(int value)
    {
        _money -= value;

        OnChangeMoneyAction?.Invoke();
    }

    public int GetMoneyData()
    {
        return _money;
    }

    public bool IsEnoughMoney(int value)
    {
        if (_money >= value)
            return true;

        return false;
    }
    #endregion

    #region DonateMoney
    public void AddDonateMoney(int value)
    {
        _donateMoney += value;

        OnChangeDonateMoneyAction?.Invoke();
    }

    public void ConsumeDonateMoney(int value)
    {
        _donateMoney -= value;

        OnChangeDonateMoneyAction?.Invoke();
    }

    public int GetDonateMoneyData()
    {
        return _donateMoney;
    }

    public bool IsEnoughDonateMoney(int value)
    {
        if (_donateMoney >= value)
            return true;

        return false;
    }
    #endregion
}
