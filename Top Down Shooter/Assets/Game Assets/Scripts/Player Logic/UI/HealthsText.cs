using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthsText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        MainPlayer.Instance.Stats.OnChangeHealthsAction += UpdateHealths;
        UpdateHealths();
    }

    public void UpdateHealths()
    {
        (float maxHealths, float healths) = MainPlayer.Instance.Stats.GetHealthsData();
        _text.text = $"HP {(int)healths}/{(int)maxHealths}";
    }
}
