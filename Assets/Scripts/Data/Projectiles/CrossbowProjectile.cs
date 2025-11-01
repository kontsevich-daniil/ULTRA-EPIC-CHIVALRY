using Data.Bullet;
using Data.Interfaces;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Data.Projectiles
{
    public class CrossbowProjectile: ProjectileData
    {
        protected override void OnTargetCollision(Collision collision, IDamageable damageable)
        {
            base.OnTargetCollision(collision, damageable);
            damageable.TakeDamage(Damage);
        }

        protected override void OnAnyCollision(Collision collision)
        {
            base.OnAnyCollision(collision);
            Rigidbody.isKinematic = true;
        }
    }
}