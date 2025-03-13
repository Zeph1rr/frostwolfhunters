using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zeph1rr.Core.Resources;

namespace Zeph1rr.FrostWolfHunters.Hunt
{
    public class Wave
    {
        public event EventHandler<ResourceStorage> OnWaveEnd;

        private int _waveNumber;
        private int _waveMultiplier;
        private GameData _gameData;
        private List<EnemyStatsSo> _enemyPrefabs;
        private Hunter _player;
        private List<Enemy> _spawnedEnemies = new();
        private Gameplay _compositeRoot;

        private ResourceStorage _resourceStorage;
        public ResourceStorage ResourceStorage => _resourceStorage;
        private int GetThreatLimit() => _waveMultiplier * _waveNumber;

        public Wave(List<EnemyStatsSo> enemyPrefabs, Hunter player, int waveMultiplier, GameData gameData, Gameplay compositeRoot)
        {
            _gameData = gameData;
            _waveNumber = gameData.CurrentWaveNumber;
            _waveMultiplier = waveMultiplier;
            _player = player;
            _compositeRoot = compositeRoot;
            _enemyPrefabs = enemyPrefabs;
            _resourceStorage = new(Enum.GetNames(typeof(ResourceType)));
        }

        public void StartWave()
        {
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            int remainingThreat = GetThreatLimit();


            if (_waveNumber % 5 == 0)
            {
                EnemyStatsSo boss = GetRandomBoss(remainingThreat);
                if (boss != null)
                {
                    remainingThreat -= boss.ThreatLevel;
                    SpawnEnemy((CreatureList) Enum.Parse(typeof(CreatureList), boss.name));
                }
            }

            while (remainingThreat > 0)
            {
                EnemyStatsSo enemyToSpawn = GetRandomEnemy(remainingThreat);
                if (enemyToSpawn == null) break;

                remainingThreat -= enemyToSpawn.ThreatLevel;
                SpawnEnemy((CreatureList) Enum.Parse(typeof(CreatureList), enemyToSpawn.name));
            }

            Debug.Log($"Wave {_waveNumber} started! Total enemies: {_spawnedEnemies.Count}");
        }

        public void EndWave()
        {
            Debug.Log("End wave!");
            _gameData.IncreaseWaveNumber();
            OnWaveEnd?.Invoke(this, _resourceStorage);
        }

        private void HandlePause(object sender, EventArgs e)
        {
            foreach (Enemy enemy in _spawnedEnemies)
            {
                if (!enemy.IsDead)
                {
                    enemy.TogglePause();
                }
            }
        }

        public void CheckWaveEnd(object sender, EventArgs e)
        {
            if (_spawnedEnemies.All(enemy => enemy.IsDead)) EndWave();
        }

        private EnemyStatsSo GetRandomBoss(int maxThreat)
        {
            List<EnemyStatsSo> possibleBosses = _enemyPrefabs.FindAll(enemy => enemy.IsBoss && enemy.ThreatLevel <= maxThreat);
            return possibleBosses.Count > 0 ? possibleBosses[UnityEngine.Random.Range(0, possibleBosses.Count)] : null;
        }

        private EnemyStatsSo GetRandomEnemy(int maxThreat)
        {
            List<EnemyStatsSo> possibleEnemies = _enemyPrefabs.FindAll(enemy => !enemy.IsBoss && enemy.ThreatLevel <= maxThreat);
            return possibleEnemies.Count > 0 ? possibleEnemies[UnityEngine.Random.Range(0, possibleEnemies.Count)] : null;
        }

        private void SpawnEnemy(CreatureList enemyPrefab)
        {
            Vector2 spawnPosition = GetRandomSpawnPosition();
            Enemy newEnemy = new(enemyPrefab, _player, _resourceStorage, spawnPosition);

            _spawnedEnemies.Add(newEnemy);
            newEnemy.OnDeath += CheckWaveEnd;
        }

        private Vector2 GetRandomSpawnPosition()
        {
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
            return (Vector2)_player.CreatureBehaviour.Transform.position + randomDirection * 5f;
        }
    }
}
