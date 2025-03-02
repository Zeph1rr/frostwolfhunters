using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zeph1rr.Core.Recources;

public class Wave : MonoBehaviour
{
    public event EventHandler<ResourceStorage> OnWaveEnd;

    private int _waveNumber;
    private int _waveMultiplier;
    private GameData _gameData;
    private List<Enemy> _enemyPrefabs;
    private Player _player;
    private List<Enemy> _spawnedEnemies = new();
    private Gameplay _compositeRoot;

    private ResourceStorage _resourceStorage = new(Enum.GetNames(typeof(ResourceType)));
    public ResourceStorage ResourceStorage => _resourceStorage;

    private int GetThreatLimit() => _waveMultiplier * _waveNumber;

    public void Initialize(List<Enemy> enemyPrefabs, Player player, int waveMultiplier, GameData gameData, Gameplay compositeRoot)
    {
        _enemyPrefabs = enemyPrefabs;
        _player = player;
        _gameData = gameData;
        _waveNumber = _gameData.CurrentWaveNumber;
        _waveMultiplier = waveMultiplier;
        _compositeRoot = compositeRoot;
        _compositeRoot.OnPausePressed += HandlePause;
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
        OnWaveEnd?.Invoke(this, _resourceStorage);
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
        _compositeRoot.OnPausePressed -= HandlePause;
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
        newEnemy.Initialize(_player, _resourceStorage); 

        
        EnemyVisual enemyVisual = newEnemy.GetComponentInChildren<EnemyVisual>();
        if (enemyVisual != null)
        {
            enemyVisual.Initialize(newEnemy);
        }

        EnemyMeleeAttack enemyAttack = newEnemy.GetComponentInChildren<EnemyMeleeAttack>();
        if (enemyAttack != null)
        {
            enemyAttack.Initialize(newEnemy);
        }

        _spawnedEnemies.Add(newEnemy); // Добавляем врага в список
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        return (Vector2)_player.transform.position + randomDirection * 5f;
    }
}
