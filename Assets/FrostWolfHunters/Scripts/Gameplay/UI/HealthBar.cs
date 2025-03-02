using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    private Player _player;
    private Image _healthBar;

    public void Initialize(Player player) {
        _healthBar = GetComponent<Image>();
        _player = player;
        _player.OnHealthChanged += HandleHealthChanged;
        _healthBar.fillAmount = _player.CurrentHealth / player.CharacterStats.GetStatValue(PlayerStats.StatNames.MaxHealth);
    }

    private void OnDestroy() {
        if (_player != null)
            _player.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(object sender, StatChangedArgs e)
    {
        _healthBar.fillAmount = e.CurrentValue / e.MaxValue;
    }
}
