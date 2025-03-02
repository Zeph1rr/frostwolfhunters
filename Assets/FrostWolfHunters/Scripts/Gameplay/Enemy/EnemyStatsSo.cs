using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BaseEnemyStats", menuName = "Stats/EnemyStats")]
public class EnemyStatsSo : ScriptableObject
{
    public float MaxHealth;
    public float CurrentHealth;
    public float Damage;
    public float AttackSpeed;
    public float AttackRange;
    public float Speed;
    public float Defence;
    public int ThreatLevel;
    public bool IsBoss;
    public ResourceType Resource;
    public int ResourceCount;

    public void Initialize(int maxHealth, int damage, float attackSpeed, float attackRange, float speed, int defence, int threatLevel, bool isBoss, ResourceType resource, int resourceCount) 
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        Damage = damage;
        AttackSpeed = attackSpeed;
        AttackRange = attackRange;
        Speed = speed;
        Defence = defence;
        ThreatLevel = threatLevel;
        IsBoss = isBoss;
        Resource = resource;
        ResourceCount = resourceCount;
    }

    public void Initialize(EnemyStatsSo newStats) {
        MaxHealth = newStats.MaxHealth;
        CurrentHealth = newStats.MaxHealth;
        Damage = newStats.Damage;
        AttackSpeed = newStats.AttackSpeed;
        AttackRange = newStats.AttackRange;
        Speed = newStats.Speed;
        Defence = newStats.Defence;
        ThreatLevel = newStats.ThreatLevel;
        IsBoss = newStats.IsBoss;
        Resource = newStats.Resource;
        ResourceCount = newStats.ResourceCount;
    }

    public event Action<float, float> OnHealthChanged;

    public void TakeDamage(float damage) {
        if (damage < 0)
        {
            throw new ArgumentOutOfRangeException("Damage cannot be negative");
        }
        float oldHealth = CurrentHealth;
        CurrentHealth = Math.Max(CurrentHealth - damage, 0);
        OnHealthChanged?.Invoke(oldHealth, CurrentHealth);
    }
}
