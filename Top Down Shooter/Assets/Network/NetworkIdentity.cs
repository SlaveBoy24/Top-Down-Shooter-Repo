using TMPro;
using UnityEngine;

public class NetworkIdentity : MonoBehaviour
{
    public delegate void InitInformationEventHandler(PlayerData data);
    public event InitInformationEventHandler InitInformationEvent;
    [HideInInspector]public bool IsInited = false;

    public PlayerData Player = new PlayerData();
    [SerializeField] private GameObject _usernamePanel;

    [HideInInspector]public static NetworkIdentity Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void ShowUsernamePanel(bool value)
    {
        _usernamePanel.SetActive(value);
    }

    public void InitInformation()
    {
        InitInformationEvent?.Invoke(Player);
        IsInited = true;
        //ProfileUsername.text = Player.username;
    }

    public void CheckUsername(TMP_InputField inputField)
    {
        string text = inputField.text;

        if (text.Length <= 3)
            return;

        PlayerData data = new PlayerData();
        data.id = Player.id;
        data.mail = Player.mail;
        data.username = text;

        NetworkClient.Instance.GetSocket()
            .Emit("username_set", new JSONObject(JsonUtility.ToJson(data)));
    }
}
