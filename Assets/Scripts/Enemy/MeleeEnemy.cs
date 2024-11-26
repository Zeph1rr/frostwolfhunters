using System.Data.Common;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MeleeEnemy : Enemy
{
    [SerializeField] Transform _target;
    [SerializeField] private State _currentState = State.Idle;
    private Rigidbody2D _rigidBody;

    private enum State {
        Idle,
        Chasing,
        Attacking,
        Dead
    }

    private void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        stats.Stats.CurrentHealth = stats.Stats.MaxHealth;
        OnTakeHit += HandleOnTakaHit;
        OnDeath += HandleOnDeath;
        if (_target == null) {
             GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) {
                _target = player.transform;
            }
             
        }
    }

    private void OnDestroy() {
        OnTakeHit -= HandleOnTakaHit;
        OnDeath -= HandleOnDeath;
    }

    protected override void Update() {
        base.Update();
        StateHandler();
    }

    private void StateHandler() {
        ChangeStateByDistance();
        switch (_currentState) {
            case State.Chasing:
                Chase();
                break;
            case State.Attacking:
                Attack();
                break;
            default:
            case State.Idle:
                break;
        }
    }

    private void ChangeStateByDistance() {
        float distanceToTarget = Vector2.Distance(_target.position, transform.position);
        if (_currentState != State.Dead) {
            if (distanceToTarget > stats.Stats.AttackRange) {
                _currentState = State.Chasing;
            } else {
                _currentState = State.Attacking;
            }
        }
        
    }
    private void Chase() {
        Vector2 direction = (_target.position - transform.position).normalized;
        _rigidBody.MovePosition(_rigidBody.position + direction * (stats.Stats.Speed * Time.deltaTime));
    }

    private void HandleOnTakaHit(object sender, System.EventArgs e) {
        Debug.Log("Took hit. Current health: " + stats.Stats.CurrentHealth);
    }

    private void HandleOnDeath(object sender, System.EventArgs e) {
        Debug.Log("Died");
        Destroy(gameObject);
    }


}
