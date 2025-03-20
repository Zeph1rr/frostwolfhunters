using System.Collections;
using FrostWolfHunters.Scripts.Game.Data;
using FrostWolfHunters.Scripts.Game.Data.Enums;
using UnityEngine;
using Zeph1rr.Core.Monos;
using Zeph1rr.FrostWolfHunters.Hunt;

namespace FrostWolfHunters.Scripts.Hunt.Weapon
{
    class PlayerWeapon: Zeph1rr.Core.Monos.Mono
    {
        [SerializeField] private WeaponList _name;
        private Hunter _player;
        private PolygonCollider2D _attackCollider;
        private PlayerStats _playerStats;

        public WeaponList Name => _name;

        public void Initialize(Hunter player, PlayerStats playerStats)
        {
            _player = player;
            _playerStats = playerStats;
            _attackCollider = GetComponent<PolygonCollider2D>();
            _player.OnPlayerAttack += HandlePlayerAttack;
        }

        private void OnDestroy()
        {
            _player.OnPlayerAttack -= HandlePlayerAttack;
        }

        private void HandlePlayerAttack(object sender, float attackSpeed)
        {
            StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(0.20f);
            _attackCollider.enabled = true;
            yield return new WaitForSeconds(0.15f);
            _attackCollider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            float damage = _playerStats.GetStatValue(StatNames.Damage);
            System.Random random = new();
            if (random.NextDouble() * (1.0 - 0.0) + 0.0 <= _playerStats.GetStatValue(StatNames.CritChance))
            {
                damage *= _playerStats.GetStatValue(StatNames.CritMultiplyer);
            }
            CreatureBehaviour behaviour = collision.GetComponent<CreatureBehaviour>();
            if (behaviour != null)
            {
                if (behaviour.ParentObject.GetType() == typeof(Zeph1rr.FrostWolfHunters.Hunt.Enemy))
                {
                    Zeph1rr.FrostWolfHunters.Hunt.Enemy enemy = (Zeph1rr.FrostWolfHunters.Hunt.Enemy)behaviour.ParentObject;
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
