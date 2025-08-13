using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] private MainPlayer _player;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private GameObject _mainUiPanel;
    [SerializeField] private bool _loaded;

    [SerializeField] private bool _loadAutoTEST;

    private void Start()
    {
        if (_loadAutoTEST)
            SetupGameData();
    }

    public void SetupGameData()
    {
        if (!_loaded)
        {
            if (_player.Initialize())
            {
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
