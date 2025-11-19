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
        [SerializeField] private Collider _playerCollider;
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
        }
        
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            Debug.Log($"Player health - {_currentHealth}");
            
            if(_currentHealth <= 0)
                Die();
        }

        public void ResetPlayer()
        {
            _currentHealth = _maxHealth;
            _playerCollider.enabled = true;
        }

        public void Die()
        {
            _playerCollider.enabled = false;
            _gameController.PlayerDied.Execute();
        }
    }
}