[System.Serializable]
public struct PlayerStats {
    public int MaxHealth;
    public int CurrentHealth;
    public int MaxStamina;
    public int CurrentStamina;
    public int Damage;
    public float AttackSpeed;
    public float Speed;
    public int Defence;

    public PlayerStats(int maxHealth, int maxStamina, int damage, int attackSpeed, int speed, int defence) {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        MaxStamina = maxStamina;
        CurrentStamina = maxStamina;
        Damage = damage;
        AttackSpeed = attackSpeed;
        Speed = speed;
        Defence = defence;
    }
}
