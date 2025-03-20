

using System;
using System.Collections;
using FrostWolfHunters.Scripts.Game.Data.Enums;
using UnityEngine;
using Zeph1rr.Core.Monos;
using Zeph1rr.Core.Resources;

namespace Zeph1rr.FrostWolfHunters.Hunt
{
    public class Enemy: Creature
    {
        public event EventHandler OnTakeHit;
        public event EventHandler OnDeath;

        private Hunter _player;
        private Transform _target;
        private ResourceStorage _resourceStorage;
        private EnemyStatsSo _stats;
        private CreatureBehaviour _creatureBehaviour;
        public CreatureBehaviour CreatureBehavoiur => _creatureBehaviour;

        private float _attackCooldownTimer = 0f;
        private bool _isPlayerDied = false;

        private bool _isRunning = false;
        public bool IsRunning => _isRunning;

        private bool _isDead = false;
        public bool IsDead => _isDead;

        private const string ATTACK = "Attack";
        private const string IS_RUNNING = "IsRunning";
        private const string IS_DEAD = "IsDead";

        private enum State
        {
            Idle,
            Chasing,
            Attacking,
            Dead
        }

        public bool IsBoss => _stats.IsBoss;
        public int ThreatLevel => _stats.ThreatLevel;

        private State _currentState = State.Chasing;


        public Enemy(CreatureList prefabName, Hunter player, ResourceStorage resourceStorage, Vector3 spawnPosition)
        {
            Debug.Log(prefabName);
            _player = player;
            _target = player.CreatureBehaviour.Transform;
            _player.OnPlayerDied += HandlePlayerDie;
            _stats = AssetsStorage.Instance.GetStats(prefabName);
            _resourceStorage = resourceStorage;
            Array values = Enum.GetValues(typeof(ResourceType));
            System.Random random = new();
            _stats.Resource = (ResourceType)values.GetValue(random.Next(values.Length));
            _creatureBehaviour = (CreatureBehaviour) AssetsStorage.Instance.CreateObject(prefabName, spawnPosition, Quaternion.identity);
            _creatureBehaviour.SetParentScript(this);
            _creatureBehaviour.SetLoop(Loop, null);
        }

        public void TogglePause()
        {
            if (_currentState == State.Idle && !_isPlayerDied)
            {
                ChangeState(State.Chasing);
            }
            else
            {
                ChangeState(State.Idle);
            }
        }

        private void HandlePlayerDie(object sender, EventArgs e)
        {
            _isRunning = false;
            ChangeState(State.Idle);
            _isPlayerDied = true;
        }

        private void Loop()
        {
            if (_isDead) return;
            if (_attackCooldownTimer > 0)
            {
                _attackCooldownTimer = Math.Max(_attackCooldownTimer - Time.deltaTime, 0);
            }
            ChangeFacingDirection(_creatureBehaviour.Transform.position, _target.position);
            StateHandler();
        }

        private bool IsFacingTarget()
        {
            float directionToTarget = Math.Abs(_target.position.x - _creatureBehaviour.Transform.position.x);
            return directionToTarget > 0 && _creatureBehaviour.Transform.localScale.x > 0;
        }


        private void StateHandler()
        {
            if (_target == null) return;

            float horizontalDistance = Mathf.Abs(_target.position.x - _creatureBehaviour.Transform.position.x);
            float verticalDistance = Mathf.Abs(_target.position.y - _creatureBehaviour.Transform.position.y);

            _isRunning = _currentState == State.Chasing;
            _creatureBehaviour.Animator.SetBool(IS_RUNNING, _isRunning);

            switch (_currentState)
            {
                case State.Idle:
                    return;
                case State.Chasing:
                    if (horizontalDistance <= _stats.AttackRange && verticalDistance <= _stats.AttackRange / 3f && IsFacingTarget())
                    {
                        ChangeState(State.Attacking);
                    }
                    else
                    {
                        Chase();
                    }
                    break;
                case State.Attacking:
                    if (horizontalDistance > _stats.AttackRange || verticalDistance > _stats.AttackRange / 3f || !IsFacingTarget())
                    {
                        ChangeState(State.Chasing);
                    }
                    else
                    {
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

        private void Chase()
        {
            if (_target != null)
            {
                Vector2 direction = (_target.position - _creatureBehaviour.Transform.position).normalized;
                _creatureBehaviour.RigidBody.MovePosition(_creatureBehaviour.RigidBody.position + direction * (_stats.Speed * Time.deltaTime));
            }
        }

        private void Attack()
        {
            if (_attackCooldownTimer <= 0)
            {
                _attackCooldownTimer = _stats.AttackSpeed;
                _creatureBehaviour.AttackBehaviour.Attack<Hunter>(_stats.AttackRange, _stats.Damage);
                _creatureBehaviour.Animator.SetTrigger(ATTACK);
            }
        }

        public override void TakeDamage(float damage)
        {
            if (_isDead) return;
            _stats.TakeDamage(damage);
            OnTakeHit?.Invoke(this, EventArgs.Empty);
            if (_stats.CurrentHealth <= 0)
            {
                Die();
            }
            _creatureBehaviour.StartCoroutine(TakeHit());
        }

        private void Die()
        {
            _isDead = true;
            ChangeState(State.Dead);
            _resourceStorage.AddResource(_stats.Resource.ToString(), _stats.ResourceCount);
            _creatureBehaviour.StartCoroutine(DestroyEnemy());
            _creatureBehaviour.Animator.SetTrigger(IS_DEAD);
            _creatureBehaviour.CapsuleCollider.enabled = false;
            OnDeath?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(5f);
            _creatureBehaviour.SelfDestroy();
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

        private IEnumerator TakeHit()
        {
            _creatureBehaviour.SpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.4f);
            _creatureBehaviour.SpriteRenderer.color = Color.white;
        }
    }
}
