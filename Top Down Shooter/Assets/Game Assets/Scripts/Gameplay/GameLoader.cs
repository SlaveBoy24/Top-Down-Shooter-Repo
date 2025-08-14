using UnityEngine;

public class GameLoader : Loader
{
    [SerializeField] private TeammatesPanelManager _teammatePanelManager;

    public override void SetupGameData()
    {
        if (!_loaded)
        {
            if (_player.Initialize())
            {
                _teammatePanelManager.SetupPanels();
                _loadingPanel.SetActive(false);
                _mainUiPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Player loading error!");
            }
        }
    }
}
