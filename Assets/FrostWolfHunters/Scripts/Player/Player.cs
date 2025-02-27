using UnityEngine;
using System;

[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    public event EventHandler OnPlayerDied;
    public event EventHandler<float> OnPlayerAttack;
    

    [SerializeField] Weapon _weaponPrefab;
    private Weapon _weapon;

    public string WeaponName => _weapon.Name;

    private PlayerStatsSO _characterStats;

    private Rigidbody2D _rigidBody;
    private GameInput _gameInput;
    private Gameplay _compositeRoot;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;
    private bool _isDead = false;
    private bool _isPaused = false;
    private Vector2 _movementVector;

    private float _attackCooldownTimer = 0;


    public bool IsRunning() 
    {
        return _isRunning;
    }

    public void Initialize(PlayerStatsSO stats, GameInput gameInput, Gameplay compositeRoot) 
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _characterStats = stats;
        _characterStats.CurrentHealth = _characterStats.MaxHealth;
        _characterStats.CurrentStamina = _characterStats.MaxStamina;
        _gameInput = gameInput;
        _gameInput.OnAttackPressed += Attack; 
        _compositeRoot = compositeRoot;
        _compositeRoot.OnPausePressed += TogglePause;
        _weapon = Instantiate(_weaponPrefab, transform);
        _weapon.Initialize(this, _characterStats.Damage);
    }

    private void TogglePause(object sender, EventArgs e)
    {
        _isPaused = !_isPaused;
        _isRunning = false;
    }

    private void OnDestroy()
    {
        _gameInput.OnAttackPressed -= Attack;
        _compositeRoot.OnPausePressed -= TogglePause;
    }

    private void Update() 
    {
        if (_isPaused || _isDead) return;
        _movementVector = _gameInput.GetMovementVector();
        Vector3 mousePos = _gameInput.GetMousePosition();
        ChangeFacingDirection(transform.position, Camera.main.ScreenToWorldPoint(mousePos));
        if (_attackCooldownTimer != 0) 
        {
            _attackCooldownTimer = Math.Max(_attackCooldownTimer - Time.deltaTime, 0);
        }
    }

    private void FixedUpdate()
    {
        if (_isPaused || _isDead) return;
        if (Mathf.Abs(_movementVector.x) > _minMovingSpeed || Mathf.Abs(_movementVector.y) > _minMovingSpeed) 
        {
            _isRunning = true;
            Move(_movementVector);
        } else {
            _isRunning = false;
        }
    }

    private void Die() 
    {
        OnPlayerDied?.Invoke(this, EventArgs.Empty);
        _isDead = true;
        _gameInput.OnAttackPressed -= Attack;
    }

    public void Move(Vector2 direction) 
    {
        direction = direction.normalized;
        _rigidBody.MovePosition(_rigidBody.position + direction * (_characterStats.Speed * Time.deltaTime));
    }

    public void TakeDamage(int damage) 
    {
        if (!_isDead)
        {
            _characterStats.TakeDamage(damage);
            if (_characterStats.CurrentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Attack(object sender, EventArgs e) 
    {
        if (_isPaused) return;
        if (_attackCooldownTimer > 0) return;
        if (!_characterStats.UseStamina(10)) Die();
        OnPlayerAttack?.Invoke(this, _characterStats.AttackSpeed);
        _attackCooldownTimer = _characterStats.AttackSpeed;
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition) 
    {
        if (sourcePosition.x > targetPosition.x) 
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        } else {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}