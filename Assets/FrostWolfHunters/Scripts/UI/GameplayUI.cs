using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameplayUI : MonoBehaviour
{
    public event EventHandler OnNewWavePressed;

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Image _cooldownImage; 
    [SerializeField] private TextMeshProUGUI _currentWaveText;
    [SerializeField] private TextMeshProUGUI _maxWaveText;
    [SerializeField] private Image _weaponImage;
    [SerializeField] private GameObject _winMenu;
    
    private GameData _gameData;
    private float _attackCooldown;
    private float _attackSpeed;
    private Player _player;
    private Gameplay _compositeRoot;

    public void Initialize(Gameplay compositeRoot, Player player, GameData gameData)
    {
        _compositeRoot = compositeRoot;
        _compositeRoot.OnPausePressed += HandlePause;
        _player = player;
        _player.OnPlayerAttack += HandleAttack;
        _gameData = gameData;
        _weaponImage.sprite = Resources.Load<Sprite>($"Weapons/{_player.WeaponName}");
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

    public void NewWave()
    {
        OnNewWavePressed?.Invoke(this, EventArgs.Empty);
    }

    public void Play()
    {
        _compositeRoot.Unpause();
    }

    public void QuitScene() 
    {
        Debug.Log("quit scene");
        SceneManager.LoadScene("Menu");
    }

    public void ShowWinMenu()
    {
        _pauseMenu.SetActive(false);
        _compositeRoot.OnPausePressed -= HandlePause;
        _winMenu.SetActive(true);
    }
}
