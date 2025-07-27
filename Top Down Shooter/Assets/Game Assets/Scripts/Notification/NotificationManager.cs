using UnityEngine;
using Assets.SimpleLocalization.Scripts;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [SerializeField] private Transform _notificationContainer;
    [SerializeField] private GameObject _notificationPrefab;

    public void Awake()
    {
        Instance = this;
    }

    public void SendNotification(string text_key, string text_var)
    {
        string text = LocalizationManager.Localize(text_key);

        text = string.Format(text, text_var);

        NotificationPanel panel = Instantiate(_notificationPrefab, _notificationContainer).GetComponent<NotificationPanel>();
        panel.InitNotificationPanel(text);
    }

    public void SendNotification(string text_key)
    { 
        string text = LocalizationManager.Localize(text_key);

        NotificationPanel panel = Instantiate(_notificationPrefab, _notificationContainer).GetComponent<NotificationPanel>();
        panel.InitNotificationPanel(text);
    }
}
