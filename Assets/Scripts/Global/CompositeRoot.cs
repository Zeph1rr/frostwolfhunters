using UnityEngine;

public class CompositeRoot : MonoBehaviour
{
     [Header("UI Elements")]
    [SerializeField] private GameObject _uiPrefab;

     [Header("Player")]
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private PlayerStatsSO _playerStats;

    [Header ("Enemy")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private EnemyStatsSo _enemyStats;

    private Player _playerInstance;
    private Enemy _enemyInstance;

    private void Awake()
    {
        InitializePlayer();
        InitializeEnemy();
        InitializeUI();
    }

    private void InitializePlayer()
    {
        _playerInstance = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
        _playerInstance.Initialize(_playerStats);

        // Передаем Player в PlayerVisual
        PlayerVisual playerVisual = _playerInstance.GetComponentInChildren<PlayerVisual>();
        if (playerVisual != null)
        {
            playerVisual.Initialize(_playerInstance);
        }
    }

    private void InitializeEnemy() 
    {
        _enemyInstance = Instantiate(_enemyPrefab, new Vector3(-2, -2, 0), Quaternion.identity);
        _enemyInstance.Initialize(_enemyStats, _playerInstance);
    }

     private void InitializeUI()
    {
        GameObject uiInstance = Instantiate(_uiPrefab, Vector3.zero, Quaternion.identity);
        HealthBar healthBar = uiInstance.GetComponentInChildren<HealthBar>();
        // Проверим, что у нас есть ссылки на UI-элементы
        if (healthBar != null)
        {
            healthBar.Initialize(_playerStats);
        }
        else
        {
            Debug.LogError("UI elements are not assigned.");
        }
    }
}
