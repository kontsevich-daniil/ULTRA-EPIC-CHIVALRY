using Data.Interfaces;
using UnityEngine;

namespace Controllers
{
    public class PlayerController: MonoBehaviour, IDamageable
    {
        private float _maxHealth = 100;
        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }
        
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            Debug.Log($"Player health - {_currentHealth}");
        }

        public void Die()
        {
            
        }
    }
}