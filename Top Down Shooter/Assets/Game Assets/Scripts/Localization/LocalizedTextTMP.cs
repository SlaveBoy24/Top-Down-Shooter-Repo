using UnityEngine;
using Assets.SimpleLocalization.Scripts;
using TMPro;

public class LocalizedTextTMP : MonoBehaviour
{
    [SerializeField] private string _localizationKey;
    private TextMeshProUGUI _text;

    public void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        Localize();
        LocalizationManager.OnLocalizationChanged += Localize;
    }

    public void OnDestroy()
    {
        LocalizationManager.OnLocalizationChanged -= Localize;
    }

    private void Localize()
    {
        _text.text = LocalizationManager.Localize(_localizationKey);
    }
}
