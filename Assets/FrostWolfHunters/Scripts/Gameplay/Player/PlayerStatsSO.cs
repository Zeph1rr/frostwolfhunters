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
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        OnHealthChanged?.Invoke(this, new StatChangedArgs(CurrentHealth, MaxHealth));
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
        CurrentStamina -= stamina;
        OnStaminaChanged?.Invoke(this, new StatChangedArgs(CurrentStamina, MaxStamina));
        Debug.Log("Current stamina: " + CurrentStamina);
        return true;
    }
}