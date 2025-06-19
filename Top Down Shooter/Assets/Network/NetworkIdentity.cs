using TMPro;
using UnityEngine;

public class NetworkIdentity : MonoBehaviour
{
    public PlayerData Player = new PlayerData();
    public GameObject SetUsernamePanel;
    private void Start()
    {

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
