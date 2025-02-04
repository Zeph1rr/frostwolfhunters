using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BaseEnemyStats", menuName = "Stats/EnemyStats")]
public class EnemyStatsSo : ScriptableObject
{
    public int MaxHealth;
    public int CurrentHealth;
    public int Damage;
    public float AttackSpeed;
    public float AttackRange;
    public float Speed;
    public int Defence;
    public int ThreatLevel;
    public bool IsBoss;

    public void Initialize(int maxHealth, int damage, float attackSpeed, float attackRange, float speed, int defence, int threatLevel, bool isBoss) {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        Damage = damage;
        AttackSpeed = attackSpeed;
        AttackRange = attackRange;
        Speed = speed;
        Defence = defence;
        ThreatLevel = threatLevel;
        IsBoss = isBoss;
    }

    public event Action<int, int> OnHealthChanged;

    public void TakeDamage(int damage) {
        if (damage < 0)
        {
            throw new ArgumentOutOfRangeException("Damage cannot be negative");
        }
        int oldHealth = CurrentHealth;
        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(oldHealth, CurrentHealth);
    }
}
