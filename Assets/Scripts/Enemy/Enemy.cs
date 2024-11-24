using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;
    public bool IsBoss => stats.Stats.IsBoss;
    public int ThreatLevel => stats.Stats.ThreatLevel;
    
    [SerializeField] protected EnemyStatsSo stats;          
     
   // [SerializeField] private Resource dropType;   // Тип ресурса, выпадаемого врагом

    private float attackCooldownTimer = 0f; 


 //   public Resource DropType => dropType;

   // protected abstract void Attack();
   // protected abstract void Move();
    public void TakeDamage(int damage) {
        stats.Stats.CurrentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    protected void DetectDeath() {
        if (stats.Stats.CurrentHealth <= 0) {
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
  //  protected abstract void Die();
    //protected abstract void DropResources();

}
