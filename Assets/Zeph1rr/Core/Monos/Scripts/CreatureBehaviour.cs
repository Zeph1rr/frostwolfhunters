using UnityEngine;

namespace Zeph1rr.Core.Monos
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class CreatureBehaviour: MonoInterlayerLoop<Creature>
    {
        [SerializeField] private Animator _animator;
        public Animator Animator => _animator;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        [SerializeField] private Rigidbody2D _rigidBody;
        public Rigidbody2D RigidBody => _rigidBody;

        [SerializeField] private CapsuleCollider2D _capsuleCollider;
        public CapsuleCollider2D CapsuleCollider => _capsuleCollider;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
        }
    }
}
