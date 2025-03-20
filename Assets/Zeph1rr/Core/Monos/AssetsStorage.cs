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

        public Mono GetPrefab<TEnum>(TEnum prefabName)
        {
            return _prefabs.FirstOrDefault<Mono>(p => p.name == prefabName.ToString());
        }

        public Mono CreateObject<TEnum>(TEnum prefabName)
        {
            return Instantiate(GetPrefab<TEnum>(prefabName));
        }

        public Mono CreateObject<TEnum>(TEnum prefabName, Transform parent)
        {
            return Instantiate(GetPrefab<TEnum>(prefabName), parent);
        }

        public Mono CreateObject<TEnum>(TEnum prefabName, Vector3 position, Quaternion rotation)
        {
            return Instantiate(GetPrefab<TEnum>(prefabName), position, rotation);
        }

        public EnemyStatsSo GetStats<TEnum>(TEnum statsName)
        {
            EnemyStatsSo stats =  ScriptableObject.CreateInstance<EnemyStatsSo>();
            stats.Initialize(_enemyStats.FirstOrDefault(s => s.name == statsName.ToString()));
            return stats;
        }
    }
}
