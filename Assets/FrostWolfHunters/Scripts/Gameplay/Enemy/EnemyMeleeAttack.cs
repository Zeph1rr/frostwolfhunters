using System.Collections;
using UnityEngine;
using Zeph1rr.Core.Monos;
using Zeph1rr.FrostWolfHunters.Hunt;

[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyMeleeAttack : MonoBehaviour
{
    private PolygonCollider2D _attackCollider;
    private Enemy _enemy;
    private float _damage;

    public void Initialize(Enemy enemy)
    {
        _attackCollider = GetComponent<PolygonCollider2D>();

        _enemy = enemy;
        _enemy.OnAttack += HandleAttack;
    }

    private void OnDestroy()
    {
        _enemy.OnAttack -= HandleAttack;
    }

    private void HandleAttack(object sender, float damage) {
        _damage = damage;
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.2f);
        _attackCollider.enabled = true;
        yield return new WaitForSeconds(0.15f);
        _attackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (_enemy.IsDead) return;
        CreatureBehaviour behaviour = collision.GetComponent<CreatureBehaviour>();
        if (behaviour != null)
        {
            if (behaviour.ParentObject.GetType() == typeof(Hunter))
            {
                Hunter player = (Hunter)behaviour.ParentObject;
                player.TakeDamage(_damage);
            }
        }
    }
}
