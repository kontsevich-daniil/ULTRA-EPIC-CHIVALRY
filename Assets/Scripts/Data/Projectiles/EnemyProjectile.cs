using Data.Bullet;
using Data.Interfaces;
using UnityEngine;

namespace Data.Projectiles
{
    public class EnemyProjectile: ProjectileData
    {
        private float _timer = -Mathf.Infinity;
        
        private void Start()
        {
            _timer = Time.time + 5;
        }

        private void Update()
        {
            if (Time.time < _timer)
                return;
               
            DisposeProjectile();
        }
        
        protected override void OnTargetCollision(Collision collision, IDamageable damageable)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                return;
            
            base.OnTargetCollision(collision, damageable);
            damageable.TakeDamage(Damage);
            DisposeProjectile();
        }

        protected override void OnAnyCollision(Collision collision)
        {
            base.OnAnyCollision(collision);
            DisposeProjectile();
        }
    }
}