using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Enemy _enemy;

    private Transform _target;

    private const string ATTACK = "Attack";
    private const string IS_RUNNING = "IsRunning";
    
    public void Initialize(Enemy enemy, Transform target) {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _enemy = enemy;
        _target = target;

        _enemy.OnAttack += HandleAttack;
    }

    private void Update() {
        _animator.SetBool(IS_RUNNING, _enemy.IsRunning);
    }

    private void OnDestroy() {
        _enemy.OnAttack -= HandleAttack;
    }
    
    private void HandleAttack(object sender, System.EventArgs e) {
       _animator.SetTrigger(ATTACK);
    }

    public void AttackColliderTurnOff() {
        _enemy.PolygonColliderTurnOff();
    }

    public void AttackColliderTurnOffOn() {
        _enemy.PolygonColliderTurnOn();
    }
}
