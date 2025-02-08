using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
public class MeleeEnemy : Enemy
{
    public override void Initialize(EnemyStatsSo stats, Player player) {
        base.Initialize(stats, player);
    }

    private void OnDestroy() {
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(_stats.Damage);
        }
    }
}
