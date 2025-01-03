using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
public class MeleeEnemy : Enemy
{
    [SerializeField] Transform _target;
    [SerializeField] private State _currentState = State.Idle;
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _hitBox;
    private PolygonCollider2D _attackCollider;

    private enum State {
        Idle,
        Chasing,
        Attacking,
        Dead
    }

    private void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _hitBox = GetComponent<BoxCollider2D>();
        _attackCollider = GetComponent<PolygonCollider2D>();
    }

    private void Start() {
        _stats.CurrentHealth = _stats.MaxHealth;
        OnTakeHit += HandleOnTakeHit;
        OnDeath += HandleOnDeath;
        OnAttack += HandleOnAttack;
        Player.Instance.OnPlayerDied += HandlePlayerDie;
        if (_target == null) {
             GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) {
                _target = player.transform;
            }
             
        }
    }

    private void OnDestroy() {
        OnTakeHit -= HandleOnTakeHit;
        OnDeath -= HandleOnDeath;
        OnAttack -= HandleOnAttack;
        Player.Instance.OnPlayerDied -= HandlePlayerDie;
    }

    protected override void Update() {
        base.Update();
        StateHandler();
        ChangeFacingDirection(_target.position, transform.position);
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
        if (_currentState == State.Idle) {
            return;
        }
        float distanceToTarget = Vector2.Distance(_target.position, transform.position);
        if (_currentState != State.Dead) {
            if (distanceToTarget > _stats.AttackRange) {
                _currentState = State.Chasing;
                PolygonColliderTurnOff();
            } else {
                _currentState = State.Attacking;
            }
        }   
    }
    private void Chase() {
        Vector2 direction = (_target.position - transform.position).normalized;
        _rigidBody.MovePosition(_rigidBody.position + direction * (_stats.Speed * Time.deltaTime));
    }

    private void HandleOnTakeHit(object sender, System.EventArgs e) {
        Debug.Log("Took hit. Current health: " + _stats.CurrentHealth);
    }

    private void HandleOnDeath(object sender, System.EventArgs e) {
        Debug.Log("Died");
        Destroy(gameObject);
    }

    private void HandleOnAttack(object sender, System.EventArgs e) {
        PolygonColliderTurnOff();
        PolygonColliderTurnOn();
    }

    private void HandlePlayerDie(object sender, System.EventArgs e) {
        _currentState = State.Idle;
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition) {
        if (sourcePosition.x < targetPosition.x) {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        } else {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void PolygonColliderTurnOn() {
        _attackCollider.enabled = true;
    }

    private void PolygonColliderTurnOff() {
        _attackCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(_stats.Damage);
        }
    }

}
