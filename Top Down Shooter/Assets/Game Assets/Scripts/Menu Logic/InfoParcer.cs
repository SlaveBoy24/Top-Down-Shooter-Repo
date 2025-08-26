using System;
using TMPro;
using UnityEngine;

public class InfoParcer : MonoBehaviour
{
    private TMP_Text _tmp;
    public string Value;
    public bool UseFormat = false;
    private void Start()
    {
        _tmp = GetComponent<TMP_Text>();

        if (NetworkIdentity.Instance.IsInited)
            _tmp.text = UseFormat ?
            Convert.ToInt32(NetworkIdentity.Instance.Player.GetFieldValue(Value)).Format() :
            NetworkIdentity.Instance.Player.GetFieldValue(Value).ToString();

        NetworkIdentity.Instance.InitInformationEvent += Parce;
    }

    private void Parce(PlayerData data)
    {
        _tmp.text = UseFormat ?
        Convert.ToInt32(data.GetFieldValue(Value)).Format() :
        $"{data.GetFieldValue(Value)}";
    }
}

public static class Formater
{
    public static string Format(this int num) =>
        num < 1000 ? num.ToString() :
        $"{num / Math.Pow(1000, (int)(Math.Log10(num) / 3)):0.##}" +
        $"{new string('K', (int)(Math.Log10(num) / 3))}";
}
