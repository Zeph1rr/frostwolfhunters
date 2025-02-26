using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StaminaBar : MonoBehaviour
{
    private PlayerStatsSO _playerStats;
    private Image _staminaBar;

    public void Initialize(PlayerStatsSO playerStats) {
        _staminaBar = GetComponent<Image>();
        _playerStats = playerStats;
        _playerStats.OnStaminaChanged += HandleStaminaChanged;
        _staminaBar.fillAmount = (float) playerStats.CurrentStamina / playerStats.MaxStamina;
    }

    private void OnDestroy()
    {
        _playerStats.OnStaminaChanged -= HandleStaminaChanged;
    }

    private void HandleStaminaChanged(object sender, StatChangedArgs e)
    {
        Debug.Log("here");
        _staminaBar.fillAmount = (float) e.CurrentValue / e.MaxValue;
    }
}
