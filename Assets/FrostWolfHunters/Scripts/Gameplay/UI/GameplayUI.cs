using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zeph1rr.Core.Recources;


public class GameplayUI : MonoBehaviour
{
    public event EventHandler OnNewWavePressed;
    public event EventHandler<bool> OnSceneQuit;

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Image _cooldownImage; 
    [SerializeField] private TextMeshProUGUI _currentWaveText;
    [SerializeField] private TextMeshProUGUI _maxWaveText;
    [SerializeField] private Image _weaponImage;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private GameObject _loseMenu;
    [SerializeField] private GameObject _resourcePrefab;
    
    private GameData _gameData;
    private float _attackCooldown;
    private float _attackSpeed;
    private Player _player;
    private Gameplay _compositeRoot;
    private ResourceStorage _resourceStorage;

    public void Initialize(Gameplay compositeRoot, Player player, GameData gameData, ResourceStorage resourceStorage)
    {
        _compositeRoot = compositeRoot;
        _compositeRoot.OnPausePressed += HandlePause;
        _player = player;
        _player.OnPlayerAttack += HandleAttack;
        _gameData = gameData;
        _weaponImage.sprite = Resources.Load<Sprite>($"Weapons/{_player.WeaponName}");
        _resourceStorage = resourceStorage;
        SetText();
    }

    private void SetText()
    {
        _currentWaveText.text = LocalizationSystem.Translate("current_wave") + ": " + _gameData.CurrentWaveNumber;
        _maxWaveText.text = LocalizationSystem.Translate("max_wave") + ": " + _gameData.CurrentWaveNumber;
    }

    private void OnDestroy()
    {
        _compositeRoot.OnPausePressed -= HandlePause;
        _player.OnPlayerAttack -= HandleAttack;
    }

    private void HandlePause(object sender, EventArgs e)
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
    }

    private void HandleAttack(object sender, float attackSpeed) {
        _attackCooldown = attackSpeed;
        _attackSpeed = attackSpeed;
        StartCoroutine(DrawCooldown());

    }

    private IEnumerator DrawCooldown() {
        while (_attackCooldown > 0) {
            _attackCooldown = Math.Max(_attackCooldown - Time.deltaTime, 0);
            _cooldownImage.fillAmount = 1f - (_attackCooldown / _attackSpeed);
            yield return null;
        }
    }

    private void InitializeResources(GameObject menu)
    {
        GridLayoutGroup resourcesContainer = menu.GetComponentInChildren<GridLayoutGroup>();
        foreach (Transform child in resourcesContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (string resourceType in Enum.GetNames(typeof(ResourceType)))
        {
            GameObject newResource = Instantiate(_resourcePrefab, resourcesContainer.transform);
            newResource.name = resourceType;

            TextMeshProUGUI text = newResource.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = _resourceStorage.GetResourceValueByName(resourceType).ToString();
            }

            Image image = newResource.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.sprite = Resources.Load<Sprite>($"Resources/{resourceType}");
            }
        }
    }

    public void NewWave()
    {
        OnNewWavePressed?.Invoke(this, EventArgs.Empty);
    }
    public void Play()
    {
        _compositeRoot.Unpause();
    }

    public void QuitScene(bool playerLeaved) 
    {
        Debug.Log("quit scene");
        OnSceneQuit?.Invoke(this, playerLeaved);
    }

    public void ShowWinMenu()
    {
        _pauseMenu.SetActive(false);
        _compositeRoot.OnPausePressed -= HandlePause;
        InitializeResources(_winMenu);
        _winMenu.SetActive(true);
    }

    public void ShowLoseMenu()
    {
        _pauseMenu.SetActive(false);
        _compositeRoot.OnPausePressed -= HandlePause;
        InitializeResources(_loseMenu);
        _loseMenu.SetActive(true);
    }
}
