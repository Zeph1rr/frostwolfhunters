using Zeph1rr.Core.Monos;
using UnityEngine;

namespace Zeph1rr.FrostWolfHunters.Hunt
{ 
    public class HunterBehavoiur : CreatureBehaviour
    {
        [SerializeField] private AttackBehaviour _ult;
        public AttackBehaviour Ult => _ult;
    }
}
