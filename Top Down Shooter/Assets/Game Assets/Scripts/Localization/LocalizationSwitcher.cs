using UnityEngine;
using Assets.SimpleLocalization.Scripts;

public class LocalizationSwitcher : MonoBehaviour
{
    public void ChangeLocalization(string lang)
    { 
        LocalizationManager.Language = lang;
    }
}
