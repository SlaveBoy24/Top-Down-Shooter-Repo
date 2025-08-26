using UnityEngine;
using System;
using TMPro;

public class DonateMoneyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        MainPlayer.Instance.Stats.OnChangeDonateMoneyAction += UpdateDonateMoney;
        UpdateDonateMoney();
    }

    public void UpdateDonateMoney()
    {
        _text.text = $"{MainPlayer.Instance.Stats.GetDonateMoneyData()}";
    }
}
