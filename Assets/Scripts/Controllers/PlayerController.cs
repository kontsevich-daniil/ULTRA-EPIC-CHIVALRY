using System;
using Cysharp.Threading.Tasks;
using Data.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class PlayerController: MonoBehaviour, IDamageable
    {
        private GameController _gameController;
        private float _maxHealth = 100;
        private float _currentHealth;
        
        [Inject]
        private void Initialized(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Awake()
        {
            _currentHealth = _maxHealth;
            
            _gameController.LevelRestart
                .Subscribe(_ => ResetPlayer())
                .AddTo(this);
        }
        
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            Debug.Log($"Player health - {_currentHealth}");
            
            if(_currentHealth <= 0)
                Die();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Trap"))
            {
                TakeDamage(_maxHealth);
            }
        }

        public void ResetPlayer()
        {
            _currentHealth = _maxHealth;
        }

        public void Die()
        {
            _gameController.PlayerDied.Execute();
        }
    }
}