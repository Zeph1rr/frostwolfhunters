using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zeph1rr.FrostWolfHunters.Hunt;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    private Hunter _player;
    private Image _healthBar;
    private TextMeshProUGUI _text;

    public void Initialize(Hunter player) {
        _healthBar = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _player = player;
        _player.OnHealthChanged += HandleHealthChanged;
        SetVisual();
    }

    private void OnDestroy() {
        if (_player != null)
            _player.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(object sender, EventArgs e)
    {
        SetVisual();
    }

    private void SetVisual()
    {
        _healthBar.fillAmount = _player.CurrentHealth / _player.CharacterStats.GetStatValue(PlayerStats.StatNames.MaxHealth);
        _text.text = $"{_player.CurrentHealth}/{_player.CharacterStats.GetStatValue(PlayerStats.StatNames.MaxHealth)}";
    }
}
