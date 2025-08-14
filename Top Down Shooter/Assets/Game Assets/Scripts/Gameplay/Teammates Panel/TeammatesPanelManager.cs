using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TeammatesPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject _teammatesPanelPrefab;
    [SerializeField] private Transform _panelContainer;

    [SerializeField] private List<TeammatePanel> _teammatesPanels;

    [SerializeField] private bool _initialized;

    public void SetupPanels()
    {
        if (_initialized)
            return;

        Room room = PhotonNetwork.CurrentRoom;

        if (room.PlayerCount > 1)
        {
            foreach (KeyValuePair<int, Player> entry in room.Players)
            {
                Player player = entry.Value;
                if (player == null)
                    continue;

                if (player == PhotonNetwork.LocalPlayer)
                    continue;

                TeammatePanel teammatePanel = Instantiate(_teammatesPanelPrefab, _panelContainer).GetComponent<TeammatePanel>();
                teammatePanel.SetupPanel(player);
                _teammatesPanels.Add(teammatePanel);
            }
        }

        _initialized = true;
    }

    public IEnumerator UpdateTeammatePanel(Player player)
    {
        yield return new WaitUntil(() => _initialized == true);
        if (player != PhotonNetwork.LocalPlayer)
        {
            foreach (TeammatePanel panel in _teammatesPanels)
            {
                if (panel.CheckPlayer(player))
                    panel.UpdateUI();
            }
        }
    }
}
