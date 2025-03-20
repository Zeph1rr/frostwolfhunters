using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FrostWolfHunters.Scripts.Game.Data;
using FrostWolfHunters.Scripts.Game.Data.Enums;
using UnityEngine;
using Zeph1rr.Core.Resources;
using Zeph1rr.Core.Utils;
using Zeph1rr.FrostWolfHunters.Hunt;

namespace FrostWolfHunters.Scripts.Hunt.Enemy
{
    public class Wave
    {
        public event EventHandler<ResourceStorage> OnWaveEnd;
        
        public ResourceStorage ResourceStorage => _resourceStorage;
        
        private readonly int _waveNumber;
        private readonly int _waveMultiplier;
        private readonly GameData _gameData;
        private readonly List<EnemyStatsSo> _enemyPrefabs;
        private readonly Hunter _player;
        private readonly List<Zeph1rr.FrostWolfHunters.Hunt.Enemy> _spawnedEnemies = new();
        private readonly ResourceStorage _resourceStorage;
        private readonly Gameplay _compositeRoot;

        private int GetThreatLimit() => _waveMultiplier * _waveNumber;

        public Wave(List<EnemyStatsSo> enemyPrefabs, Hunter player, int waveMultiplier, GameData gameData, Gameplay compositeRoot)
        {
            _gameData = gameData;
            _waveNumber = gameData.CurrentWaveNumber;
            _waveMultiplier = waveMultiplier;
            _player = player;
            _enemyPrefabs = enemyPrefabs;
            _resourceStorage = new(Enum.GetNames(typeof(ResourceType)));
            _compositeRoot = compositeRoot;
            _compositeRoot.OnPausePressed += HandlePause;
        }

        public void StartWave()
        {
            Coroutines.StartRoutine(SpawnEnemies());
        }

        public void StopAllCoroutines()
        {
            foreach (var enemy in _spawnedEnemies.Where(enemy => !enemy.IsDead))
            {
                enemy.CreatureBehavoiur.StopAllCoroutines();
            }
        }

        private IEnumerator SpawnEnemies()
        {
            int remainingThreat = GetThreatLimit();


            if (_waveNumber % 5 == 0)
            {
                EnemyStatsSo boss = GetRandomBoss(remainingThreat);
                if (boss != null)
                {
                    remainingThreat -= boss.ThreatLevel;
                    SpawnEnemy((CreatureList) Enum.Parse(typeof(CreatureList), boss.name));
                    yield return new WaitForSeconds(0.2f);
                }
            }

            while (remainingThreat > 0)
            {
                EnemyStatsSo enemyToSpawn = GetRandomEnemy(remainingThreat);
                if (enemyToSpawn == null) break;

                remainingThreat -= enemyToSpawn.ThreatLevel;
                SpawnEnemy((CreatureList) Enum.Parse(typeof(CreatureList), enemyToSpawn.name));
                yield return new WaitForSeconds(0.2f);
            }

            Debug.Log($"Wave {_waveNumber} started! Total enemies: {_spawnedEnemies.Count}");
        }

        private void EndWave()
        {
            Debug.Log("End wave!");
            _gameData.IncreaseWaveNumber();
            _compositeRoot.OnPausePressed -= HandlePause;
            OnWaveEnd?.Invoke(this, _resourceStorage);
        }

        private void HandlePause(object sender, EventArgs e)
        {
            foreach (var enemy in _spawnedEnemies.Where(enemy => !enemy.IsDead))
            {
                enemy.TogglePause();
            }
        }

        private void CheckWaveEnd(object sender, EventArgs e)
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
            Zeph1rr.FrostWolfHunters.Hunt.Enemy newEnemy = new(enemyPrefab, _player, _resourceStorage, spawnPosition);

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
