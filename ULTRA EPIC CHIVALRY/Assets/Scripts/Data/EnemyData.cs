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

        private void AgentSetup()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _moveSpeed;
            _agent.stoppingDistance = _attackRange * 0.8f;
        }
    }
}