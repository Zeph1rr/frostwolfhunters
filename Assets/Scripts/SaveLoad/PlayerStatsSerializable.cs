using UnityEngine;

[System.Serializable]
public class PlayerStatsSerializable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxStamina;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _speed;
    [SerializeField] private int _defence;

    public PlayerStatsSerializable(PlayerStatsSO stats) {
        _maxHealth = stats.MaxHealth;
        _maxStamina = stats.MaxStamina;
        _damage = stats.Damage;
        _attackSpeed = stats.AttackSpeed;
        _speed = stats.Speed;
        _defence = stats.Defence;
    }

    public PlayerStatsSO Deserialize(PlayerStatsSO stats) {
        stats.Initialize(_maxHealth, _maxStamina,  _damage, _attackSpeed, _speed, _defence);
        return stats;
    }
}
