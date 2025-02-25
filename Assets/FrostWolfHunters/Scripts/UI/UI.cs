using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UI : MonoBehaviour
{

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Image _cooldownImage; 
    [SerializeField] private TextMeshProUGUI _currentWaveText;
    [SerializeField] private TextMeshProUGUI _maxWaveText;
    
    private GameInput _gameInput;
    private GameData _gameData;
    private float _attackCooldown;
    private float _attackSpeed;
    private Player _player;

    public void Initialize(GameInput gameInput, Player player, GameData gameData)
    {
        _gameInput = gameInput;
        _gameInput.OnPausePressed += HandlePause;
        _player = player;
        _player.OnPlayerAttack += HandleAttack;
        _gameData = gameData;
        SetText();
    }

    private void SetText()
    {
        _currentWaveText.text = LocalizationSystem.Translate("current_wave") + ": " + _gameData.CurrentWaveNumber;
        _maxWaveText.text = LocalizationSystem.Translate("max_wave") + ": " + _gameData.CurrentWaveNumber;
    }

    private void OnDestroy()
    {
        _gameInput.OnPausePressed -= HandlePause;
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

    public void Play()
    {
        _gameInput.Unpause();
    }

    public void QuitScene() 
    {
        Debug.Log("quit scene");
        SceneManager.LoadScene("Menu");
    }
}
