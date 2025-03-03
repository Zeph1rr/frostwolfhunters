using UnityEngine;
using System;

[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    public event EventHandler OnPlayerDied;
    public event EventHandler<float> OnPlayerAttack;

    public event EventHandler<StatChangedArgs> OnHealthChanged;
    public event EventHandler<StatChangedArgs> OnStaminaChanged;
    

    [SerializeField] Weapon _weaponPrefab;
    private Weapon _weapon;

    public string WeaponName => _weapon.Name;

    private PlayerStats _characterStats;
    public PlayerStats CharacterStats => _characterStats;
    private float _currentHealth;
    private float _currentStamina;
    public float CurrentHealth => _currentHealth;
    public float CurrentStamina => _currentStamina;

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

    public void Initialize(PlayerStats playerStats, GameInput gameInput, Gameplay compositeRoot) 
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _characterStats = playerStats;
        _currentHealth = _characterStats.GetStatValue(PlayerStats.StatNames.MaxHealth);
        _currentStamina = _characterStats.GetStatValue(PlayerStats.StatNames.MaxStamina);
        _gameInput = gameInput;
        _gameInput.OnAttackPressed += Attack; 
        _compositeRoot = compositeRoot;
        _compositeRoot.OnPausePressed += TogglePause;
        _weapon = Instantiate(_weaponPrefab, transform);
        _weapon.Initialize(this, _characterStats);
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
        _rigidBody.MovePosition(_rigidBody.position + _characterStats.GetStatValue(PlayerStats.StatNames.Speed) * Time.deltaTime * direction);
    }

    public void TakeDamage(float damage) 
    {
        if (damage < 0)
        {
            throw new ArgumentException("Damage cannot be negative");
        }
        if (!_isDead)
        {
            float decreasedDamage = Mathf.Max(damage - _characterStats.GetStatValue(PlayerStats.StatNames.Defence), 0);
            _currentHealth = Mathf.Max(_currentHealth - decreasedDamage);
            OnHealthChanged?.Invoke(this, new StatChangedArgs(_currentHealth, _characterStats.GetStatValue(PlayerStats.StatNames.MaxHealth)));
            Debug.Log("Current health: " + _currentHealth);
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private bool UseStamina(float stamina)
    {
        if (stamina < 0) 
        {
            throw new ArgumentOutOfRangeException("Stamina cannot be negative");
        }
        if (_currentStamina - stamina < 0) {
            Debug.LogWarning("Not enough stamina!");
            return false;
        }
        float beforeStamina = _currentStamina;
        _currentStamina -= stamina;
        OnStaminaChanged?.Invoke(this, new StatChangedArgs(_currentStamina, _characterStats.GetStatValue(PlayerStats.StatNames.MaxStamina)));
        Debug.Log("Current stamina: " + _currentStamina);
        return true;
    }

    private void Attack(object sender, EventArgs e) 
    {
        if (_isPaused) return;
        if (_attackCooldownTimer > 0) return;
        if (!UseStamina(10)) Die();
        OnPlayerAttack?.Invoke(this, _characterStats.GetStatValue(PlayerStats.StatNames.AttackSpeed));
        _attackCooldownTimer = _characterStats.GetStatValue(PlayerStats.StatNames.AttackSpeed);
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

public class StatChangedArgs : EventArgs
{
    public StatChangedArgs(float currentValue, float maxValue) 
    {
        CurrentValue = currentValue;
        MaxValue = maxValue;
    }

    public float CurrentValue;
    public float MaxValue;
}