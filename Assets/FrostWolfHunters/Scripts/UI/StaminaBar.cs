using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StaminaBar : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO _playerStats;
    private Image _staminaBar;

    private void Awake()
    {
        _staminaBar = GetComponent<Image>();
    }

    private void Update()
    {
        _staminaBar.fillAmount = (float) _playerStats.CurrentStamina / _playerStats.MaxStamina;
    }
}
