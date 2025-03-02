using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StaminaBar : MonoBehaviour
{
    private Player _player;
    private Image _staminaBar;

    public void Initialize(Player player) 
    {
        _staminaBar = GetComponent<Image>();
        _player = player;
        _player.OnStaminaChanged += HandleStaminaChanged;
        _staminaBar.fillAmount = _player.CurrentStamina / _player.CharacterStats.GetStatValue(PlayerStats.StatNames.MaxStamina);
    }

    private void OnDestroy()
    {
        _player.OnStaminaChanged -= HandleStaminaChanged;
    }

    private void HandleStaminaChanged(object sender, StatChangedArgs e)
    {
        _staminaBar.fillAmount = e.CurrentValue / e.MaxValue;
    }
}
