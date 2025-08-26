using UnityEngine;
using TMPro;
using System;

public class MoneyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        MainPlayer.Instance.Stats.OnChangeMoneyAction += UpdateMoney;
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        int money = MainPlayer.Instance.Stats.GetMoneyData();
        if (money >= 100000)
            _text.text = Convert.ToInt32(money).Format();
        else
            _text.text = $"{money}";
    }
}
