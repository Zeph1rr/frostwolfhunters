using System.Collections;
using UnityEngine;
using Zeph1rr.Core.Monos;

namespace Zeph1rr.FrostWolfHunters.Hunt
{
    public class MeleeAttack : AttackBehaviour
    {
        public override void Attack<T>(float attackRange, float damage, Transform target = null)
        {
            StartCoroutine(AttackRoutine<T>(attackRange, damage));
        }

        private IEnumerator AttackRoutine<T>(float attackRange, float damage) where T: Creature
        {
            yield return new WaitForSeconds(0.15f);
            Vector2 attackCenter = transform.position + (transform.right + transform.up) * attackRange;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCenter, attackRange);
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