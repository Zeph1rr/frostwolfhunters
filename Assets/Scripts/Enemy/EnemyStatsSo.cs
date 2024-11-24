using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyStats", menuName = "Stats/EnemyStats")]
public class EnemyStatsSo : ScriptableObject
{
    public EnemyStats Stats;

    public void Initialize(EnemyStats newStats) {
        Stats = newStats;
    }
}
