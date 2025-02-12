using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class CompositeRoot : MonoBehaviour
{
    [Header("GameData")]
    [SerializeField] private GameData _gameData;

    [Header("UI Elements")]
    [SerializeField] private GameObject _uiPrefab;

    [Header("Player")]
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private PlayerStatsSO _playerStats;

    [Header ("Enemy")]
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] private Wave _wave;
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
        _waveInstance.Initialize(_enemyPrefabs, _playerInstance, _waveMultiplier, _gameData);
        _waveInstance.OnWaveEnd += HandleWaveEnd;
        _waveInstance.StartWave();
    }

    private void HandleWaveEnd(object sender, EventArgs e) {
        NewWave();
    }

    private void NewWave() {
        _waveInstance.OnWaveEnd -= HandleWaveEnd;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
