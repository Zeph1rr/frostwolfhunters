using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zeph1rr.FrostWolfHunters.Hunt;

[RequireComponent(typeof(Image))]
public class StaminaBar : MonoBehaviour
{
    private Hunter _player;
    private Image _staminaBar;
    private TextMeshProUGUI _text;

    public void Initialize(Hunter player) 
    {
        _staminaBar = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _player = player;
        _player.OnStaminaChanged += HandleStaminaChanged;
        SetVisual();
    }

    private void OnDestroy()
    {
        _player.OnStaminaChanged -= HandleStaminaChanged;
    }

    private void HandleStaminaChanged(object sender, EventArgs e)
    {
        SetVisual();
    }

    private void SetVisual()
    {
        _staminaBar.fillAmount = _player.CurrentStamina / _player.CharacterStats.GetStatValue(PlayerStats.StatNames.MaxStamina);
        _text.text = $"{_player.CurrentStamina}/{_player.CharacterStats.GetStatValue(PlayerStats.StatNames.MaxStamina)}";
    }
}
