using Data.Bullet;
using Data.Interfaces;
using UnityEngine;

namespace Data.Projectiles
{
    public class KnifeProjectile: ProjectileData
    {
        protected override void OnTargetCollision(Collision collision, IDamageable damageable)
        {
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