using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class TeammatePanel : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TextMeshProUGUI _nickname;

    public void SetupPanel(Player player)
    { 
        _player = player;
        _nickname.text = _player.NickName;

        UpdateUI();
    }

    public bool CheckPlayer(Player player)
    {
        if (_player == player)
            return true;

        return false;
    }

    public void UpdateUI()
    {
        ExitGames.Client.Photon.Hashtable playerProperties = _player.CustomProperties;

        if (!playerProperties.ContainsKey("Healths") || !playerProperties.ContainsKey("MaxHealths"))
        {
            _hpSlider.maxValue = 100;
            _hpSlider.value = 0;
            return;
        }
        else
        {
            _hpSlider.maxValue = (float)playerProperties["MaxHealths"];
            _hpSlider.value = (float)playerProperties["Healths"];
        }
    }
}
