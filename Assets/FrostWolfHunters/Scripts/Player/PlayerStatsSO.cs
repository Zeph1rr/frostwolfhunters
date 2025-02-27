using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BasePlayerStats", menuName = "Stats/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    public event EventHandler<StatChangedArgs> OnHealthChanged;
    public event EventHandler<StatChangedArgs> OnStaminaChanged;
    public int MaxHealth;
    public int CurrentHealth;
    public int MaxStamina;
    public int CurrentStamina;
    public int Damage;
    public float AttackSpeed;
    public float Speed;
    public int Defence;

    public void Initialize(int maxHealth, int maxStamina, int damage, float attackSpeed, float speed, int defence) 
    {
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
        int beforeHealth = CurrentHealth;
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        OnHealthChanged?.Invoke(this, new StatChangedArgs(beforeHealth, CurrentHealth, MaxHealth));
        Debug.Log("Current health: " + CurrentHealth);
    }

    public bool UseStamina(int stamina) {
        if (stamina < 0) 
        {
            throw new ArgumentOutOfRangeException("Stamina cannot be negative");
        }
        if (CurrentStamina - stamina < 0) {
            Debug.LogWarning("Not enough stamina!");
            return false;
        }
        int beforeStamina = CurrentStamina;
        CurrentStamina -= stamina;
        OnStaminaChanged?.Invoke(this, new StatChangedArgs(beforeStamina, CurrentStamina, MaxStamina));
        Debug.Log("Current stamina: " + CurrentStamina);
        return true;
    }
}


public class StatChangedArgs : EventArgs
{
    public StatChangedArgs(int beforeValue, int currentValue, int maxValue) 
    {
        CurrentValue = currentValue;
        MaxValue = maxValue;
        BeforeValue = beforeValue;
    }

    public int CurrentValue;
    public int MaxValue;
    public int BeforeValue;
}