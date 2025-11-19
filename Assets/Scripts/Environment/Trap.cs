using Data.Interfaces;
using UnityEngine;
using Zenject.SpaceFighter;

namespace Environment
{
    public class Trap: MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(999);
            }
        }
    }
}