using Zeph1rr.Core.Monos;
using UnityEngine;
using System;
using FrostWolfHunters.Scripts.Game.Data;
using FrostWolfHunters.Scripts.Game.Data.Enums;
using FrostWolfHunters.Scripts.Hunt;

namespace Zeph1rr.FrostWolfHunters.Hunt
{
    public class Hunter : Creature
    {
        public event EventHandler OnHealthChanged;
        public event EventHandler OnStaminaChanged;
        public event EventHandler OnPlayerDied;
        public event EventHandler<float> OnPlayerAttack;

        public float CurrentHealth => _currentHealth;
        public float CurrentStamina => _currentStamina;
        public HunterBehavoiur CreatureBehaviour => _creatureBehaviour;
        public WeaponList WeaponName => _weaponName;
        public PlayerStats CharacterStats => _characterStats;

        private readonly float _minMovingSpeed = 0.1f;
        private const string IS_RUNNING = "IsRunning";
        private const string IS_DEAD = "IsDead";

        private HunterBehavoiur _creatureBehaviour;
        private GameInput _gameInput;
        private PlayerStats _characterStats;
        private Gameplay _compositeRoot;

        private bool _isRunning = false;
        private bool _isPaused = false;
        private bool _isDead = false;
        private float _attackCooldownTimer = 0;
        private Vector3 _movementVector;
        private WeaponList _weaponName;

        private float _currentHealth;
        private float _currentStamina;



        public Hunter(GameInput gameInput, PlayerStats characterStats, Gameplay compositeRoot)
        {
            _creatureBehaviour = (HunterBehavoiur) AssetsStorage.Instance.CreateObject<CreatureList>(CreatureList.Hunter, Vector3.zero, Quaternion.identity);
            _creatureBehaviour.SetParentScript(this);
            _creatureBehaviour.SetLoop(Loop, FixedLoop);

            _gameInput = gameInput;
            _gameInput.OnAttackPressed += Attack;
            _gameInput.OnUltPressed += Ult;

            _compositeRoot = compositeRoot;
            _compositeRoot.OnPausePressed += TogglePause;

            _characterStats = characterStats;
            _currentHealth = _characterStats.GetStatValue(StatNames.MaxHealth);
            _currentStamina = _characterStats.GetStatValue(StatNames.MaxStamina);

            _weaponName = WeaponList.Axe;

        }

        public override void TakeDamage(float damage)
        {
            if (damage < 0)
            {
                throw new ArgumentException("Damage cannot be negative");
            }
            if (!_isDead)
            {
                float decreasedDamage = Mathf.Max(damage - _characterStats.GetStatValue(StatNames.Defence), 0);
                _currentHealth = Mathf.Max(_currentHealth - decreasedDamage);
                OnHealthChanged?.Invoke(this, EventArgs.Empty);
                Debug.Log("Current health: " + _currentHealth);
                if (_currentHealth <= 0)
                {
                    Die();
                }
            }
        }

        private void Attack(object sender, EventArgs e)
        {
            if (_isPaused) return;
            if (_attackCooldownTimer > 0) return;
            if (!UseStamina(10)) Die();
            OnPlayerAttack?.Invoke(this, _characterStats.GetStatValue(StatNames.AttackSpeed));
            float damage = _characterStats.GetStatValue(StatNames.Damage);
            System.Random random = new();
            if (random.NextDouble() * (1.0 - 0.0) + 0.0 <= _characterStats.GetStatValue(StatNames.CritChance))
            {
                damage *= _characterStats.GetStatValue(StatNames.CritMultiplyer);
            }
            _creatureBehaviour.AttackBehaviour.Attack<Enemy>(0.75f, damage);
            _attackCooldownTimer = _characterStats.GetStatValue(StatNames.AttackSpeed);
            _creatureBehaviour.Animator.SetTrigger($"{_weaponName.ToString().ToUpper()}_ATTACK");
        }

        private void Ult(object sender, EventArgs e)
        {
            if (_isPaused) return;
            if (!UseStamina(50)) Die();
            float damage = _characterStats.GetStatValue(StatNames.Damage) * _characterStats.GetStatValue(StatNames.CritMultiplyer);
            _creatureBehaviour.Ult.Attack<Enemy>(0.75f, damage);
            _creatureBehaviour.Animator.SetTrigger($"{_weaponName.ToString().ToUpper()}_ULT");
        }

        private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
        {
            if (sourcePosition.x > targetPosition.x)
            {
                _creatureBehaviour.Transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                _creatureBehaviour.Transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        private void Die()
        {
            OnPlayerDied?.Invoke(this, EventArgs.Empty);
            _isDead = true;
            _creatureBehaviour.Animator.SetBool(IS_DEAD, _isDead);
            _gameInput.OnAttackPressed -= Attack;
        }

        private void FixedLoop()
        {
            if (_isPaused || _isDead) return;
            if (Mathf.Abs(_movementVector.x) > _minMovingSpeed || Mathf.Abs(_movementVector.y) > _minMovingSpeed)
            {
                _isRunning = true;
                Move(_movementVector);
            }
            else
            {
                _isRunning = false;
            }
            _creatureBehaviour.Animator.SetBool(IS_RUNNING, _isRunning);
        }

        private void Loop()
        {
            if (_isPaused || _isDead) return;
            _movementVector = _gameInput.GetMovementVector();
            Vector3 mousePos = _gameInput.GetMousePosition();
            ChangeFacingDirection(_creatureBehaviour.Transform.position, Camera.main.ScreenToWorldPoint(mousePos));
            if (_attackCooldownTimer != 0)
            {
                _attackCooldownTimer = Mathf.Max(_attackCooldownTimer - Time.deltaTime, 0);
            }
        }

        private void Move(Vector2 direction)
        {
            direction = direction.normalized;
            _creatureBehaviour.RigidBody.MovePosition(_creatureBehaviour.RigidBody.position + _characterStats.GetStatValue(StatNames.Speed) * Time.deltaTime * direction);
        }

        private void TogglePause(object sender, EventArgs e)
        {
            _isPaused = !_isPaused;
            _isRunning = false;
        }

        private bool UseStamina(float stamina)
        {
            if (stamina < 0)
            {
                throw new ArgumentOutOfRangeException("Stamina cannot be negative");
            }
            if (_currentStamina - stamina < 0)
            {
                Debug.LogWarning("Not enough stamina!");
                return false;
            }
            _currentStamina -= stamina;
            OnStaminaChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log("Current stamina: " + _currentStamina);
            return true;
        }
    }
}