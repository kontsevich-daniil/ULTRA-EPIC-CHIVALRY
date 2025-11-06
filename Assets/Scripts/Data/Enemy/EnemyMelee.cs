using System;
using System.Threading;
using Controllers;
using Cysharp.Threading.Tasks;
using Data.Interfaces;
using Enemy;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Data.Enemy
{
    public class EnemyMelee : EnemyData
    {
        [SerializeField] private Transform _player;
        Animator anim;

        private bool _isActive = true;

        [Inject]
        private void Initialized(GameController gameController, PlayerController playerController)
        {
            _player = playerController.transform;

            gameController.LevelCompleted
                .Merge(gameController.PlayerDied)
                .Subscribe(_ => { _isActive = false; })
                .AddTo(_disposables);
        }

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_isDead || _player == null || !_isActive)
                return;

            base.Update();

            float distance = Vector3.Distance(transform.position, _player.position);

            if (!_agent.enabled)
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Vector3 origin = transform.position;

            Gizmos.DrawSphere(origin, _detectionRange);
        }
    }
}