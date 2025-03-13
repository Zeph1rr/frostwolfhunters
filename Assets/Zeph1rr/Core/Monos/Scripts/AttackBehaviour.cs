using UnityEngine;

namespace Zeph1rr.Core.Monos
{
    public abstract class AttackBehaviour : Mono
    {
        public abstract void Attack<T>(float attackRange, float damage, Transform target = null) where T : Creature;
    }
}
