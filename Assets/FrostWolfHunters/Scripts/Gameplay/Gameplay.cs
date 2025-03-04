using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;
using Zeph1rr.Core.Resources;

public class Gameplay : MonoBehaviour, ISceneCompositeRoot
{
    [Header("GameData")]
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

    public event EventHandler OnPausePressed;

    private Player _playerInstance;
    private Wave _waveInstance;
    private GameplayUI _uiInstance;
    private GameData _gameData;

    private bool _waveFinished;
    
    public void StartScene(GameData gameData)
    {
        _gameData = gameData;
        InitializePlayer();
        InitializeEnemy();
        InitializeUI();
        InitializeCamera();
        // КОСТЫЛЬ ПЕРЕДЕЛАТЬ
        _waveFinished = false;
    }

    public void Unpause()
    {
        OnPausePressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        _uiInstance.OnNewWavePressed -= HandleNewWavePressed;
        _uiInstance.OnSceneQuit -= HandleSceneQuit;

        _gameInput.OnPausePressed -= HandlePausePressed;

        _waveInstance.OnWaveEnd -= HandleWaveEnd;

        _playerInstance.OnPlayerDied -= HandlePlayerDie;
    }

    private void InitializePlayer()
    {
        _gameInput.Initialize();
        _gameInput.OnPausePressed += HandlePausePressed;
        _playerInstance = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
        _playerInstance.Initialize(_gameData.PlayerStats,_gameInput, this);

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
        _waveInstance.Initialize(_enemyPrefabs, _playerInstance, _waveMultiplier, _gameData, this);
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

    private void HandleWaveEnd(object sender, ResourceStorage resourceStorage) {
        if (_waveFinished) return;
        _waveFinished = true;
        _gameData.HuntResourceStorage.AddResources(resourceStorage.Resources);
        OnPausePressed?.Invoke(this, EventArgs.Empty);
        _uiInstance.ShowWinMenu();
    }

    private void NewWave() {
        _waveInstance.OnWaveEnd -= HandleWaveEnd;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void InitializeUI()
    {
        GameObject uiInstance = Instantiate(_uiPrefab, Vector3.zero, Quaternion.identity);
        _uiInstance = uiInstance.GetComponent<GameplayUI>();
        _uiInstance.Initialize(this, _playerInstance, _gameData, _gameData.HuntResourceStorage);
        HealthBar healthBar = uiInstance.GetComponentInChildren<HealthBar>();
        StaminaBar staminaBar = uiInstance.GetComponentInChildren<StaminaBar>();
        if (staminaBar != null && healthBar != null)
        {
            healthBar.Initialize(_playerInstance);
            staminaBar.Initialize(_playerInstance);
        }
        else
        {
            Debug.LogError("UI elements are not assigned.");
        }
        _uiInstance.OnNewWavePressed += HandleNewWavePressed;
        _uiInstance.OnSceneQuit += HandleSceneQuit;
    }

    private void HandlePlayerDie(object sender, EventArgs e) {  
        if (_waveFinished) return;
        _waveFinished = true;      
        OnPausePressed?.Invoke(this, EventArgs.Empty);
        _gameData.HuntResourceStorage.AddResources(_waveInstance.ResourceStorage.Resources);
        _gameData.Die();
        _uiInstance.ShowLoseMenu();
    }

    private void HandleNewWavePressed(object sender, EventArgs e)
    {
        NewWave();
    }

    private void HandlePausePressed(object sender, EventArgs e) 
    {
        OnPausePressed?.Invoke(this, EventArgs.Empty);
    }

    private void HandleSceneQuit(object sender, bool playerLeaved)
    {
        if (playerLeaved)
        {
            _gameData.HuntResourceStorage.AddResources(_waveInstance.ResourceStorage.Resources);
            _gameData.HuntResourceStorage.DecreaseAll(0.5f);
            _gameData.Leave();
        }
        _gameData.ResourceStorage.AddResources(_gameData.HuntResourceStorage.Resources);
        _gameData.HuntResourceStorage.ResetResourceStorage(Enum.GetNames(typeof(ResourceType)));
        _gameData.ResourceStorage.PrintResources();
        SceneManager.LoadScene("Tribe");
    }
}
