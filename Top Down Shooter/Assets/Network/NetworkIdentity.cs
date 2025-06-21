using TMPro;
using UnityEngine;

public class NetworkIdentity : MonoBehaviour
{
    public PlayerData Player = new PlayerData();
    [SerializeField]private GameObject _usernamePanel;
    public TMP_Text ProfileUsername;
    public void ShowUsernamePanel(bool value)
    {
        _usernamePanel.SetActive(value);
    }
    public void InitInformation()
    {
        ProfileUsername.text = Player.username;
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
