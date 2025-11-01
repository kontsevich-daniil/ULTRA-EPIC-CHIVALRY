using System;
using Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject.SpaceFighter;

namespace Enemy
{
    [Serializable]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyData : MonoBehaviour
    {
        protected NavMeshAgent _agent;
        
        protected float _currentHealth;
        protected float _moveSpeed;
        protected float _attackDamage;
        protected float _detectionRange;
        protected float _attackRange;
        protected float _attackCooldown;
        
        protected bool _isDead = false;
        protected float _lastAttackTime = -Mathf.Infinity;
        
        [FormerlySerializedAs("enemyConfig")] public EnemySO enemySo;
        public EEnemy type;
        
        public float pushDecay = 8f;
        private Vector3 pushVelocity;

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

        public void KnockBackFrom(Vector3 source, float strength)
        {
            Vector3 dir = (transform.position - source);
            dir.y = 0;
            dir.Normalize();
            pushVelocity += dir * strength;
        }

        private void AgentSetup()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _moveSpeed;
            _agent.stoppingDistance = _attackRange * 0.8f;
        }
    }
}