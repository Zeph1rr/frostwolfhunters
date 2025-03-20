using UnityEngine;
using Zeph1rr.Core.Monos;

namespace Zeph1rr.FrostWolfHunters.Hunt
{
    public class MeleeUlt : AttackBehaviour
    {
        public override void Attack<T>(float attackRange, float damage, Transform target = null)
        {
            attackRange = 4f;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.TryGetComponent<CreatureBehaviour>(out var behaviour))
                {
                    if (behaviour.ParentObject.GetType() == typeof(T))
                    {
                        T damagable = (T)behaviour.ParentObject;
                        damagable.TakeDamage(damage);
                    }
                }
            }
        }
    }
}
