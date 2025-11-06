using System;
using Controllers;
using Cysharp.Threading.Tasks;
using Data.Bullet;
using Data.Interfaces;
using Enemy;
using UniRx;
using UnityEngine;
using Zenject;

namespace Data.Enemy
{
    public class EnemyShooter : EnemyData
    {
        Animator anim;
        [Header("Settings")] [SerializeField] private float detectionRange = 15f;
        [SerializeField] private float rotateSpeed = 5f;
        [SerializeField] private float shootCooldown = 1.2f;

        [Header("Shooting")] [SerializeField] private float forceShoot = 10f;
        [SerializeField] private Transform firePoint;
        [SerializeField] private ProjectileData projectilePrefab;

        private Transform _player;
        private bool _canShoot = true;

        private bool _isActive = true;

        [Inject]
        private void Construct(GameController gameController, PlayerController player)
        {
            _player = player.transform;

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

            if (distance <= detectionRange)
            {
                anim.SetBool("iswalking", false);
                RotateToPlayer();
                TryShoot();
            }
        }

        private void RotateToPlayer()
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        private void TryShoot()
        {
            if (!_canShoot) return;

            Shoot().Forget();
        }

        private async UniTaskVoid Shoot()
        {
            anim.SetBool("isshooting", true);
            anim.SetBool("iswalking", false);
            _canShoot = false;

            var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.Rigidbody.AddForce(firePoint.forward * forceShoot, ForceMode.Impulse);

            await UniTask.Delay(TimeSpan.FromSeconds(shootCooldown),
                cancellationToken: this.GetCancellationTokenOnDestroy());

            _canShoot = true;
        }
    }
}