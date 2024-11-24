using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public Stats stats;

    // Вы можете добавить методы для настройки или обработки данных, если потребуется
    public void Initialize(Stats newStats)
    {
        stats = newStats;
    }
}
