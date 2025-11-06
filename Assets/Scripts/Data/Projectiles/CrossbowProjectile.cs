using System;
using Data.Bullet;
using Data.Interfaces;
using UnityEngine;

namespace Data.Projectiles
{
    public class CrossbowProjectile: ProjectileData
    {
        [SerializeField] private LayerMask _targetLayerMask;
        
        private void OnTriggerEnter(Collider other)
        {
            if (IsProjectileDisposed)
                return;
            
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                OnTargetCollider(other, damageable);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                Rigidbody.isKinematic = true;
                Collider.isTrigger = false;
            }
        }

        private void OnTargetCollider(Collider collider, IDamageable damageable)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) 
                return;
            
            damageable.TakeDamage(Damage);
        }
    }
}