[System.Serializable]
public struct Stats 
{
    public int MaxHealth;
    public int MaxStamina;
    public int Attack;
    public int Defense;
    public float Speed;

    public Stats(int health, int stamina, int attack, int defense, float speed)
    {
        MaxHealth = health;
        MaxStamina = stamina;
        Attack = attack;
        Defense = defense;
        Speed = speed;
    }
}
