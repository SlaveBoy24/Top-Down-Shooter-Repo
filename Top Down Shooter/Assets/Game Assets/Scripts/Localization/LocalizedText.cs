using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string _textKey;
    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        LocalizationManager.Instance.OnLangChange -= ChangeText;
        LocalizationManager.Instance.OnLangChange += ChangeText;
        ChangeText();
    }

    void ChangeText()
    {
        _text.text = LocalizationManager.Instance.Get(_textKey);
    }
}
