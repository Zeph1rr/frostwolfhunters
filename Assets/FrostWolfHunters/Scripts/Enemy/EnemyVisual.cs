using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyVisual : MonoBehaviour
{
    private Animator _animator;
    private Enemy _enemy;

    private const string ATTACK = "Attack";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";
    
    public void Initialize(Enemy enemy) {
        _animator = GetComponent<Animator>();

        _enemy = enemy;

        _enemy.OnAttack += HandleAttack;
        _enemy.OnDeath += HandeDeath;
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

    private void HandeDeath(object sender, System.EventArgs e) {
        _animator.SetTrigger(IS_DEAD);
    }

    public void AttackColliderTurnOff() {
        _enemy.PolygonColliderTurnOff();
    }

    public void AttackColliderTurnOffOn() {
        _enemy.PolygonColliderTurnOn();
    }
}
