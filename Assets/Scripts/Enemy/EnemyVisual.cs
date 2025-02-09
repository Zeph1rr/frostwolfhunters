using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyVisual : MonoBehaviour
{
    private Animator _animator;
    private Enemy _enemy;

    private const string ATTACK = "Attack";
    private const string IS_RUNNING = "IsRunning";
    
    public void Initialize(Enemy enemy) {
        _animator = GetComponent<Animator>();

        _enemy = enemy;

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
