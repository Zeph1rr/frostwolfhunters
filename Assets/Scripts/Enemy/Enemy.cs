using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;
    public event EventHandler OnAttack;
    public bool IsBoss => _stats.IsBoss;
    public int ThreatLevel => _stats.ThreatLevel;
    
    [SerializeField] protected EnemyStatsSo _stats;          
     
   // [SerializeField] private Resource dropType;   // Тип ресурса, выпадаемого врагом

    private float attackCooldownTimer = 0f; 


 //   public Resource DropType => dropType;

   protected virtual void Update() {
        if (attackCooldownTimer > 0) {
            attackCooldownTimer = Math.Max(attackCooldownTimer - Time.deltaTime, 0);
        }
   }
   // protected abstract void Move();
    public void TakeDamage(int damage) {
        _stats.CurrentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    protected void DetectDeath() {
        if (_stats.CurrentHealth <= 0) {
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    protected void Attack() {
        if (attackCooldownTimer == 0) {
            Debug.Log("Attack!");
            attackCooldownTimer += _stats.AttackSpeed;
            OnAttack?.Invoke(this, EventArgs.Empty);
        }
    }
  //  protected abstract void Die();
    //protected abstract void DropResources();

}
