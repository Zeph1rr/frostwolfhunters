using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Wave : MonoBehaviour
{
    public event EventHandler OnWaveEnd;

    private int _waveNumber;
    private int _waveMultiplier;
    private GameData _gameData;
    private List<Enemy> _enemyPrefabs;
    private Player _player;
    private List<Enemy> _spawnedEnemies = new List<Enemy>();
    private GameInput _gameInput;

    private int GetThreatLimit() => _waveMultiplier * _waveNumber;

    // Инициализация через метод Initialize
    public void Initialize(List<Enemy> enemyPrefabs, Player player, int waveMultiplier, GameData gameData, GameInput gameInput)
    {
        _enemyPrefabs = enemyPrefabs;
        _player = player;
        _gameData = gameData;
        _waveNumber = _gameData.CurrentWaveNumber;
        _waveMultiplier = waveMultiplier;
        _gameInput = gameInput;
        _gameInput.OnPausePressed += HandlePause;
    }

    public void StartWave()
    {
        int remainingThreat = GetThreatLimit();

        // Если волна кратна 5, выбираем и спавним босса
        if (_waveNumber % 5 == 0)
        {
            Enemy boss = GetRandomBoss(remainingThreat);
            if (boss != null)
            {
                remainingThreat -= boss.ThreatLevel;
                SpawnEnemy(boss);
            }
        }

        // Спавним обычных врагов на оставшуюся сумму угрозы
        while (remainingThreat > 0)
        {
            Enemy enemyToSpawn = GetRandomEnemy(remainingThreat);
            if (enemyToSpawn == null) break;

            remainingThreat -= enemyToSpawn.ThreatLevel;
            SpawnEnemy(enemyToSpawn);
        }

        Debug.Log($"Wave {_waveNumber} started! Total enemies: {_spawnedEnemies.Count}");
    }

    public void EndWave() {
        Debug.Log("End wave!");
        _gameData.IncreaseWaveNumber();
        OnWaveEnd?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    private void HandlePause(object sender, EventArgs e) 
    {
        foreach(Enemy enemy in _spawnedEnemies) 
        {
            if (!enemy.IsDead) {
                enemy.TogglePause();
            }
        }
    }

    private void Update() {
        // foreach(Enemy enemy in _spawnedEnemies) {
        //     enemy.TakeDamage(1);
        // }
        if (_spawnedEnemies.All(enemy => enemy.IsDead)) EndWave();
    }

    private void OnDestroy()
    {
        _gameInput.OnPausePressed -= HandlePause;
    }

    private Enemy GetRandomBoss(int maxThreat)
    {
        List<Enemy> possibleBosses = _enemyPrefabs.FindAll(enemy => enemy.IsBoss && enemy.ThreatLevel <= maxThreat);
        return possibleBosses.Count > 0 ? possibleBosses[UnityEngine.Random.Range(0, possibleBosses.Count)] : null;
    }

    private Enemy GetRandomEnemy(int maxThreat)
    {
        List<Enemy> possibleEnemies = _enemyPrefabs.FindAll(enemy => !enemy.IsBoss && enemy.ThreatLevel <= maxThreat);
        return possibleEnemies.Count > 0 ? possibleEnemies[UnityEngine.Random.Range(0, possibleEnemies.Count)] : null;
    }

    private void SpawnEnemy(Enemy enemyPrefab)
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        Enemy newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.Initialize(_player); // Передаем Player в Initialize

        

        EnemyVisual enemyVisual = newEnemy.GetComponentInChildren<EnemyVisual>();
        if (enemyVisual != null)
        {
            enemyVisual.Initialize(newEnemy);
        }

        _spawnedEnemies.Add(newEnemy); // Добавляем врага в список
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        return (Vector2)_player.transform.position + randomDirection * 5f;
    }
}
