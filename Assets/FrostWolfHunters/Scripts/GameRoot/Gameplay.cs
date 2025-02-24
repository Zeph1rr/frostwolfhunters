using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;

public class Gameplay : MonoBehaviour, ISceeneRoot
{
    [Header("GameData")]
    [SerializeField] private GameData _gameData;
    [SerializeField] private GameInput _gameInput;

    [Header("UI Elements")]
    [SerializeField] private GameObject _uiPrefab;

    [Header("Player")]
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private PlayerStatsSO _playerStats;

    [Header ("Enemy")]
    [SerializeField] private List<Enemy> _enemyPrefabs;
    [SerializeField] private Wave _wave;
    [SerializeField] private int _waveMultiplier;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _cinemachineCamera;

    private Player _playerInstance;
    private Wave _waveInstance;

    public void StartScene()
    {
        InitializePlayer();
        InitializeEnemy();
        InitializeUI();
        InitializeCamera();
    }

    private void OnDisable() {
        _playerInstance.OnPlayerDied -= HandlePlayerDie;
    }

    private void SaveGame() {
        PlayerStatsSerializable playerStats = new PlayerStatsSerializable(_playerStats);
        GameDataSerializable gameData = new GameDataSerializable(_gameData, playerStats);
        SaveLoadSystem.SaveGame(gameData, $"{_gameData.PlayerName}.save");
    }

    private void InitializePlayer()
    {
        
        _gameInput.Initialize();
        _playerInstance = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
        _playerInstance.Initialize(_playerStats, _gameInput);

        // Передаем Player в PlayerVisual
        PlayerVisual playerVisual = _playerInstance.GetComponentInChildren<PlayerVisual>();
        if (playerVisual != null)
        {
            playerVisual.Initialize(_playerInstance);
        }
        _playerInstance.OnPlayerDied += HandlePlayerDie;
    }

    private void InitializeEnemy() 
    {
        _waveInstance = Instantiate(_wave, new Vector3(0, 0, 0), Quaternion.identity);
        _waveInstance.Initialize(_enemyPrefabs, _playerInstance, _waveMultiplier, _gameData);
        _waveInstance.OnWaveEnd += HandleWaveEnd;
        _waveInstance.StartWave();
    }

    private void InitializeCamera()
    {
        if (_cinemachineCamera == null)
        {
            _cinemachineCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
        }

        if (_cinemachineCamera != null && _playerInstance != null)
        {
            _cinemachineCamera.Follow = _playerInstance.transform;
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera or Player not found!");
        }
    }

    private void HandleWaveEnd(object sender, EventArgs e) {
        SaveGame();
        // NewWave();
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

    private void HandlePlayerDie(object sender, EventArgs e) {
        _gameInput.enabled = false;
    }
}
