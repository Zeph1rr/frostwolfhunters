using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    private PlayerStatsSO _playerStats;
    private Image _healthBar;

    public void Initialize(PlayerStatsSO playerStats) {
        _healthBar = GetComponent<Image>();
        _playerStats = playerStats;
        _playerStats.OnHealthChanged += HandleHealthChanged;
        _healthBar.fillAmount = (float) playerStats.CurrentHealth / playerStats.MaxHealth;
    }

    private void OnDestroy() {
        if (_playerStats != null)
            _playerStats.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(object sender, StatChangedArgs e)
    {
        _healthBar.fillAmount = (float) e.CurrentValue / e.MaxValue;
    }
}
