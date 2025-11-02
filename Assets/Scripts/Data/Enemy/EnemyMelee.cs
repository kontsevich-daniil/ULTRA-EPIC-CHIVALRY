using System;
using Controllers;
using Data.Interfaces;
using Enemy;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Data.Enemy
{
    public class EnemyMelee : EnemyData, IDamageable
    {
        [SerializeField] private Transform _player;
        Animator anim;

        [Inject]
        private void Initialized(PlayerController playerController)
        {
            _player = playerController.transform;
        }

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_isDead || _player == null) 
                return;
            
            base.Update();

            float distance = Vector3.Distance(transform.position, _player.position);

            if(!_agent.enabled)
                return;
            
            if (distance <= _detectionRange)
            {
                _agent.SetDestination(_player.position);

                if (distance <= _attackRange)
                {
                    TryAttack();
                }
            }
            else
            {
                //_agent.ResetPath();
            }
        }

        private void TryAttack()
        {
            if (Time.time - _lastAttackTime < _attackCooldown)
                return;

            _lastAttackTime = Time.time;

            if (_player.TryGetComponent(out IDamageable damageble))
            {
                anim.SetBool("iswalking", false);
                anim.SetBool("isattacking", true);
                damageble.TakeDamage(_attackDamage);
            }
        }

        public void TakeDamage(float damage)
        {
            if (_isDead) 
                return;

            _currentHealth -= damage;
            if (_currentHealth <= 0)
                Die();
        }

        public void Die()
        {
            _agent.isStopped = true;
            Destroy(gameObject);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Vector3 origin = transform.position;

            Gizmos.DrawSphere(origin, _detectionRange);
        }
    }
}