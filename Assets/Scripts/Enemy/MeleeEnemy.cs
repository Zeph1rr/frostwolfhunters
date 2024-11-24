
using UnityEngine;

public class MeleeEnemy : Enemy
{
    private State _currentState;

    public enum State {
        Idle,
        Chasing,
        Attacking,
        Dead
    }

    private void Start() {
        _currentState = State.Idle;
        stats.Stats.CurrentHealth = stats.Stats.MaxHealth;
        OnTakeHit += HandleOnTakaHit;
        OnDeath += HandleOnDeath;
    }

    private void Update() {
        StateHandler();
    }

    private void StateHandler() {
        switch (_currentState) {
            default:
            case State.Idle:
                break;
        }
    }

    private void HandleOnTakaHit(object sender, System.EventArgs e) {
        Debug.Log("Took hit. Current healt: " + stats.Stats.CurrentHealth);
    }

    private void HandleOnDeath(object sender, System.EventArgs e) {
        Debug.Log("Died");
        Destroy(gameObject);
    }
}
