using UnityEngine;

public class GameLoader : Loader
{
    [SerializeField] private TeammatesPanelManager _teammatePanelManager;
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private EnemySpawner _enemySpawner;

    public override void SetupGameData()
    {
        if (!_loaded)
        {

            if (_player.Initialize())
            {
                _teammatePanelManager.SetupPanels();
                if (_enemySpawner != null)
                    _enemySpawner.Initialize();

                _playerSpawner.Initialize();

                MainPlayer.Instance.Stats.UpdatePhotonPlayerProperties();
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
