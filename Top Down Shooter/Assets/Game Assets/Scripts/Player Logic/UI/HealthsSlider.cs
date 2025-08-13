using UnityEngine;
using UnityEngine.UI;

public class HealthsSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Start()
    {
        MainPlayer.Instance.Stats.OnChangeHealthsAction += UpdateHealths;
        UpdateHealths();
    }

    public void UpdateHealths()
    {
        (float maxHealths, float healths) = MainPlayer.Instance.Stats.GetHealthsData();
        _slider.maxValue = maxHealths;
        _slider.value = healths;
    }
}
