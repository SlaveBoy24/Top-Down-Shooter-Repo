using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] protected MainPlayer _player;
    [SerializeField] protected ShopManager _shopManager;
    [SerializeField] protected GameObject _loadingPanel;
    [SerializeField] protected GameObject _mainUiPanel;
    [SerializeField] protected bool _loaded;

    [SerializeField] protected bool _loadAutoTEST;

    private void Start()
    {
        if (_loadAutoTEST)
            SetupGameData();
    }

    public virtual void SetupGameData()
    {
        if (!_loaded)
        {
            if (_player.Initialize())
            {
                _shopManager.Initialize();
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
