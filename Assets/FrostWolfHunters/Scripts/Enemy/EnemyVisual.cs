using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyVisual : MonoBehaviour
{
    private Animator _animator;
    private Enemy _enemy;
    private SpriteRenderer _sprite;
    private const string ATTACK = "Attack";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";
    
    public void Initialize(Enemy enemy) 
    {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();

        _enemy = enemy;

        _enemy.OnAttack += HandleAttack;
        _enemy.OnDeath += HandeDeath;
        _enemy.OnTakeHit += HandleTakeHit;
    }

    private void Update()
     {
        _animator.SetBool(IS_RUNNING, _enemy.IsRunning);
    }

    private void OnDestroy() 
    {
        _enemy.OnAttack -= HandleAttack;
        _enemy.OnTakeHit -= HandleTakeHit;
        _enemy.OnDeath -= HandeDeath;
    }
    
    private void HandleAttack(object sender, int damage)
    {
       _animator.SetTrigger(ATTACK);
    }

    private void HandeDeath(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(IS_DEAD);
    }

    private void HandleTakeHit(object sender, System.EventArgs e)
    {
        StartCoroutine(TakeHit());
    }

    private IEnumerator TakeHit()
    {
        _sprite.color = Color.red;
        yield return new WaitForSeconds(.4f);
        _sprite.color = Color.white;
    }
}
