using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;
    public event EventHandler OnAttack;

    [SerializeField] protected EnemyStatsSo _stats;
    private Player _player;
    private Transform _target;

    private Rigidbody2D _rigidBody;
    private PolygonCollider2D _attackCollider;
    private float _attackCooldownTimer = 0f;

    private bool _isRunning = false;
    public bool IsRunning => _isRunning;
    private bool _isPlayerDied = false;

    private enum State {
        Idle,
        Chasing,
        Attacking,
        Dead
    }

    public bool IsBoss => _stats.IsBoss;
    public int ThreatLevel => _stats.ThreatLevel;

    private State _currentState = State.Chasing;

    public virtual void Initialize(Player player) {
        _rigidBody = GetComponent<Rigidbody2D>();
        _attackCollider = GetComponent<PolygonCollider2D>();

        _player = player;
        _target = player.transform;
        _player.OnPlayerDied += HandlePlayerDie;
    }

    private void OnDestroy() {
        _player.OnPlayerDied -= HandlePlayerDie;
    }

    private void HandlePlayerDie(object sender, EventArgs e) {
        _isPlayerDied = true;
        ChangeState(State.Idle);
    }

    private void Update() {
        if (_attackCooldownTimer > 0) 
        {
            _attackCooldownTimer = Math.Max(_attackCooldownTimer - Time.deltaTime, 0);
        }
        if (_isPlayerDied) return;
        ChangeFacingDirection(transform.position, _target.position);
        StateHandler();
    }

    private bool IsFacingTarget() {
        float directionToTarget = Math.Abs(_target.position.x - transform.position.x);
        return directionToTarget > 0 && transform.localScale.x > 0;
    }


    private void StateHandler() {
        if (_target == null) return;

        float horizontalDistance = Mathf.Abs(_target.position.x - transform.position.x);
        float verticalDistance = Mathf.Abs(_target.position.y - transform.position.y);

        _isRunning = _currentState == State.Chasing;

        switch (_currentState) {
            case State.Chasing:
                if (horizontalDistance <= _stats.AttackRange && verticalDistance <= 0.3f && IsFacingTarget()) { 
                    ChangeState(State.Attacking);
                } else {
                    Chase();
                }
                break;
            case State.Attacking:
                if (horizontalDistance > _stats.AttackRange || verticalDistance > 0.3f || !IsFacingTarget()) {
                    ChangeState(State.Chasing);
                } else {
                    Attack();
                }
                break;
            case State.Dead:
                break;
        }
    }


    private void ChangeState(State newState)
    {
        _currentState = newState;
    }

    private void Chase() {
        if (_target != null) {
            Vector2 direction = (_target.position - transform.position).normalized;
            _rigidBody.MovePosition(_rigidBody.position + direction * (_stats.Speed * Time.deltaTime));
        }
    }

    private void Attack() {
        if (_attackCooldownTimer <= 0)
        {
            Debug.Log("attack");
            _attackCooldownTimer = _stats.AttackSpeed;
            OnAttack?.Invoke(this, EventArgs.Empty);
        }
    }

    private void TakeDamage(int damage) {
        _stats.TakeDamage(damage);
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        if (_stats.CurrentHealth == 0) {
            Die();
        }
    }

    private void Die() {
        OnDeath?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public void PolygonColliderTurnOn() {
        _attackCollider.enabled = true;
    }

    public void PolygonColliderTurnOff() {
        _attackCollider.enabled = false;
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition) {
        if (sourcePosition.x > targetPosition.x) {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        } else {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
