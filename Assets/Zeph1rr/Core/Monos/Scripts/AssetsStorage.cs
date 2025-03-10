using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeph1rr.Core.Monos
{
    public class AssetsStorage : MonoBehaviour
    {
        public static AssetsStorage Instance { get; private set; }

        [SerializeField] private List<Mono> _prefabs;
        [SerializeField] private List<EnemyStatsSo> _enemyStats;
        public List<EnemyStatsSo> EnemyStats => _enemyStats;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;
        }

        public Mono GetPrefab<TEnum>(TEnum name)
        {
            return _prefabs.FirstOrDefault<Mono>(p => p.name == name.ToString());
        }

        public Mono CreateObject<TEnum>(TEnum name)
        {
            return Instantiate(GetPrefab<TEnum>(name));
        }

        public Mono CreateObject<TEnum>(TEnum name, Transform parent)
        {
            return Instantiate(GetPrefab<TEnum>(name), parent);
        }

        public Mono CreateObject<TEnum>(TEnum name, Vector3 position, Quaternion rotation)
        {
            return Instantiate(GetPrefab<TEnum>(name), position, rotation);
        }

        public EnemyStatsSo GetStats<TEnum>(TEnum name)
        {
            EnemyStatsSo stats =  ScriptableObject.CreateInstance<EnemyStatsSo>();
            stats.Initialize(_enemyStats.FirstOrDefault(s => s.name == name.ToString()));
            return stats;
        }
    }
}
