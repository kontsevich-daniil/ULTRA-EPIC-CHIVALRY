using System;
using System.Threading;
using Controllers;
using Cysharp.Threading.Tasks;
using Data.Interfaces;
using Enums;
using ScriptableObjects;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;
using Zenject.SpaceFighter;

namespace Enemy
{
    [Serializable]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyData : MonoBehaviour, IDamageable, IDisposable
    {
        protected NavMeshAgent _agent;
        protected Rigidbody _rigidbody;
        [SerializeField] protected SpriteRenderer _model;
        [SerializeField] protected Billboard _billboard;
        protected Transform _player;
        
        protected bool _isActive = true;
        
        protected CompositeDisposable _disposables = new();
        
        protected float _currentHealth;
        protected float _moveSpeed;
        protected float _attackDamage;
        protected float _detectionRange;
        protected float _attackRange;
        protected float _attackCooldown;
        
        protected bool _isDead = false;
        protected float _lastAttackTime = -Mathf.Infinity;
        
        private Vector3 pushVelocity;
        
        public EnemySO enemySo;
        public EEnemy type;
        public float pushDecay = 8f;
        
        [Inject]
        private void Construct(GameController gameController, PlayerController playerController)
        {
            _player = playerController.transform;
            
            gameController.LevelCompleted
                .Merge(gameController.PlayerDied)
                .Subscribe(_ => { _isActive = false; })
                .AddTo(_disposables);
        }

        protected void Awake()
        {
            var enemy = enemySo.GetEnemy(type);

            _currentHealth = enemy.MaxHealth;
            _moveSpeed = enemy.MoveSpeed;
            _attackDamage = enemy.AttackDamage;
            _detectionRange = enemy.DetectionRange;
            _attackRange = enemy.AttackRange;
            _attackCooldown = enemy.AttackCooldown;
            
            AgentSetup();
        }

        protected void Update()
        {
            if (pushVelocity.sqrMagnitude > 0.001f)
            {
                _agent.Move(pushVelocity * Time.deltaTime);

                pushVelocity = Vector3.Lerp(pushVelocity, Vector3.zero, pushDecay * Time.deltaTime);
            }
        }

        public async UniTaskVoid KnockBackFrom(Vector3 source, float strength)
        {
            _billboard.enabled = false;
            _agent.enabled = false;
            _rigidbody.AddForce((transform.position - source) * strength, ForceMode.Impulse);
            await UniTask.Delay(TimeSpan.FromSeconds(3));
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _agent.enabled = true;
            _billboard.enabled = true;
        }

        private void AgentSetup()
        {
            _agent = GetComponent<NavMeshAgent>();
            _rigidbody = GetComponent<Rigidbody>();
            _agent.speed = _moveSpeed;
            _agent.stoppingDistance = _attackRange * 0.8f;
        }

        
        public void TakeDamage(float damage)
        {
            if (_isDead) 
                return;

            _currentHealth -= damage;
            AnimDamage().Forget();
            if (_currentHealth <= 0)
                Die();
        }

        private async UniTask AnimDamage()
        {
            _model.color = Color.red;
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _model.color = Color.white;
        }

        public void Die()
        {
            _agent.isStopped = true;
            Destroy(gameObject);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }

        public abstract UniTaskVoid Shoot();
    }
}