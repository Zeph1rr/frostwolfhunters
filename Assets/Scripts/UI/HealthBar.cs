using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO _playerStats;
    private Image _healthBar;

    private void Awake()
    {
        _healthBar = GetComponent<Image>();
    }

    private void Update()
    {
        _healthBar.fillAmount = (float) _playerStats.CurrentHealth / _playerStats.MaxHealth;
    }
}
