using System;
using Configs;
using Data.Interfaces;
using ScriptableObjects;
using UnityEngine;

namespace Data.Weapons
{
    public class MagicHands : WeaponData
    {
        private float lastShootTime = -Mathf.Infinity;
        
        [Header("Cone Settings")] 
        public float maxRadius = 10f;

        [Range(0, 180)] public float coneAngle = 45f;
        
        public LayerMask enemyLayer;

        public override void ShootFirstType()
        {
            if (Time.time < lastShootTime + _cooldownFirstAttack)
                return;

            lastShootTime = Time.time;
            
            Collider[] results = new Collider[5];
            Physics.OverlapSphereNonAlloc(transform.position, maxRadius, results, enemyLayer);

            foreach (var hit in results)
            {
                Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;

                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                if (angleToTarget <= coneAngle && hit.TryGetComponent(out IDamageable damageble))
                {
                    damageble.TakeDamage(_damage);
                }
            }
            Debug.Log("Magic Hands Soot First");
        }

        public override void ShootSecondType()
        {
        }

        protected override void PlaySound()
        {
        }

        protected override void EnableVFX()
        {
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.1f, 0.3f, 0.9f, 0.8f);
            Vector3 forward = transform.forward;
            
            int segments = 4;
            float stepAngle = coneAngle * 2 / segments;
            Vector3 origin = transform.position;

            Vector3 previousPoint = origin + Quaternion.Euler(0, -coneAngle, 0) * forward * maxRadius;
            for (int i = 0; i <= segments; i++)
            {
                float currentAngle = -coneAngle + stepAngle * i;
                Vector3 nextPoint = origin + Quaternion.Euler(0, currentAngle, 0) * forward * maxRadius;
                Gizmos.DrawLine(origin, nextPoint);
                Gizmos.DrawLine(previousPoint, nextPoint);
                previousPoint = nextPoint;
            }
        }
    }
}