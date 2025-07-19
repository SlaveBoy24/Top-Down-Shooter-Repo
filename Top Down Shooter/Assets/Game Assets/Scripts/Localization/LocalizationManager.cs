using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    private Dictionary<string, string> localizedText;
    public string currentLanguage = "en";
    public Action OnLangChange;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLocalization();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeLang(string lang)
    {
        currentLanguage = lang;

        LoadLocalization();
    }

    private void LoadLocalization()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("localization");
        localizedText = new Dictionary<string, string>();

        string[] lines = csvFile.text.Split('\n');
        string[] headers = lines[0].Split(';');
        int langIndex = System.Array.IndexOf(headers, currentLanguage);
        for (int i = 0; i < headers.Length; i++)
            Debug.Log(headers[i]);

        Debug.Log(langIndex);

        for (int i = 1; i < lines.Length; i++)
        {
            var cells = lines[i].Split(';');
            if (cells.Length > langIndex)
                localizedText[cells[0]] = cells[langIndex];
        }

        OnLangChange?.Invoke();
    }

    public string Get(string key)
    {
        return localizedText.ContainsKey(key) ? localizedText[key] : key;
    }
}