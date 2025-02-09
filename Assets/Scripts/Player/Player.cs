using UnityEngine;
using System;

[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player Instance {get; private set;}

    public event EventHandler OnPlayerDied;
    public event EventHandler<StatChangedArgs> OnHealthChanged;

    private PlayerStatsSO _characterStats;

    private Rigidbody2D _rigidBody;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;
    private bool _isDead = false;
    private Vector2 _movementVector;


    public bool IsRunning() {
        return _isRunning;
    }

    public void Initialize(PlayerStatsSO stats) {
        _rigidBody = GetComponent<Rigidbody2D>();
        _characterStats = stats;
        _characterStats.CurrentHealth = _characterStats.MaxHealth;
        _characterStats.CurrentStamina = _characterStats.MaxStamina;
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        OnHealthChanged?.Invoke(
                this, 
                new StatChangedArgs(
                    _characterStats.CurrentHealth,
                    _characterStats.CurrentHealth, 
                    _characterStats.MaxHealth
                    )
            );
    }

    private void Update() {
        _movementVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate() {
        if (Mathf.Abs(_movementVector.x) > _minMovingSpeed || Mathf.Abs(_movementVector.y) > _minMovingSpeed) {
            _isRunning = true;
            Move(_movementVector);
        } else {
            _isRunning = false;
        }
    }

    private void Die() {
        OnPlayerDied?.Invoke(this, EventArgs.Empty);
        _isDead = true;
    }

    public void Move(Vector2 direction) {
        direction = direction.normalized;
        _rigidBody.MovePosition(_rigidBody.position + direction * (_characterStats.Speed * Time.deltaTime));
    }

    public Vector3 GetPlayerScreenPosition() {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public void TakeDamage(int damage) {
        if (!_isDead)
        {
            _characterStats.TakeDamage(damage);
            if (_characterStats.CurrentHealth <= 0)
            {
                Die();
            }
        }
    }

}