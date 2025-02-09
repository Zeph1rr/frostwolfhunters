using UnityEngine;
using System.Collections.Generic;

public class CompositeRoot : MonoBehaviour
{
     [Header("UI Elements")]
    [SerializeField] private GameObject _uiPrefab;

     [Header("Player")]
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private PlayerStatsSO _playerStats;

    [Header ("Enemy")]
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] private Wave _wave;
    [SerializeField] private int _waveNumber;
    [SerializeField] private int _waveMultiplier;

    private Player _playerInstance;
    private Wave _waveInstance;

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
        _waveInstance = Instantiate(_wave, new Vector3(0, 0, 0), Quaternion.identity);
        _waveInstance.Initialize(_enemyPrefabs, _playerInstance, _waveNumber, _waveMultiplier);
        _waveInstance.StartWave();
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
