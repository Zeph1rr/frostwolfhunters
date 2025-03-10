using UnityEngine;
using System.Collections;

namespace Zeph1rr.FrostWolfHunters.Hunt
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
            Enemy enemy = collision.GetComponent<Enemy>();
            float damage = _playerStats.GetStatValue(PlayerStats.StatNames.Damage);
            System.Random random = new();
            if (random.NextDouble() * (1.0 - 0.0) + 0.0 <= _playerStats.GetStatValue(PlayerStats.StatNames.CritChance))
            {
                damage *= _playerStats.GetStatValue(PlayerStats.StatNames.CritMultiplyer);
            }
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
