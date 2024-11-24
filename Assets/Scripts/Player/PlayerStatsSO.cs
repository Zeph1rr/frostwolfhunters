using UnityEngine;

[CreateAssetMenu(fileName = "BasePlayerStats", menuName = "Stats/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    public PlayerStats Stats;

    public void Initialize(PlayerStats newStats) {
        Stats = newStats;
    }
}
