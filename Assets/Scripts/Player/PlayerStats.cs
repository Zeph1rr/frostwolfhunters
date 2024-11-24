using UnityEngine;

public class PlayerStats : MonoBehaviour
{
   [Header("Character Stats")]
    [SerializeField] private CharacterStats _characterStats;

    // Текущие характеристики
    private int _currentHealth;
    private int _currentStamina;

    private void Awake()
    {
        // Инициализация текущих характеристик на основе ScriptableObject
        ResetStats();
    }


    public void ResetStats()
    {
        _currentHealth = _characterStats.stats.MaxHealth;
        _currentStamina = _characterStats.stats.MaxStamina;
    }

    public void TakeDamage(int damage)
    {
        int effectiveDamage = Mathf.Max(damage - _characterStats.stats.Defense, 0);
        _currentHealth -= effectiveDamage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _characterStats.stats.MaxHealth);
    }

    public void UseStamina(int amount)
    {
        _currentStamina = Mathf.Max(_currentStamina - amount, 0);
    }

    public void RecoverStamina(int amount)
    {
        _currentStamina = Mathf.Min(_currentStamina + amount, _characterStats.stats.MaxStamina);
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Обработка смерти
    }

    public int CurrentHealth => _currentHealth;
    public int CurrentStamina => _currentStamina;
}
