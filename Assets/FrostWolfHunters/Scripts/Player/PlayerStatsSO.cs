using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BasePlayerStats", menuName = "Stats/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    public event EventHandler<StatChangedArgs> OnHealthChanged;
    public int MaxHealth;
    public int CurrentHealth;
    public int MaxStamina;
    public int CurrentStamina;
    public int Damage;
    public float AttackSpeed;
    public float Speed;
    public int Defence;

    public void Initialize(int maxHealth, int maxStamina, int damage, float attackSpeed, float speed, int defence) {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        MaxStamina = maxStamina;
        CurrentStamina = maxStamina;
        Damage = damage;
        AttackSpeed = attackSpeed;
        Speed = speed;
        Defence = defence;
    }

    public void Initialize(PlayerStatsSO playerStats)
    {
        MaxHealth = playerStats.MaxHealth;
        CurrentHealth = MaxHealth;
        MaxStamina = playerStats.MaxStamina;
        CurrentStamina = MaxStamina;
        Damage = playerStats.Damage;
        AttackSpeed = playerStats.AttackSpeed;
        Speed = playerStats.Speed;
        Defence = playerStats.Defence;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            throw new ArgumentOutOfRangeException("Damage cannot be negative");
        }
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        OnHealthChanged?.Invoke(this, new StatChangedArgs(CurrentHealth - damage, CurrentHealth, MaxHealth));
        Debug.Log("Current health: " + CurrentHealth);
    }
}


public class StatChangedArgs : EventArgs
{
    public StatChangedArgs(int beforeValue, int currentValue, int maxValue) {
        CurrentValue = currentValue;
        MaxValue = maxValue;
        BeforeValue = beforeValue;
    }

    public int CurrentValue;
    public int MaxValue;
    public int BeforeValue;
}