using System;
using Configs;
using Data.Interfaces;
using ScriptableObjects;
using UnityEngine;
using IDamageable = Data.Interfaces.IDamageable;

namespace Data.Weapons
{
    public class MagicHands : WeaponData
    {
        [SerializeField] private float _damage = 20;
        [Header("Cone Settings")] 
        [SerializeField] private float maxRadius = 10f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask obstacleLayer;

        [Range(0, 180)] 
        [SerializeField] private float coneAngle = 45f;

        private Collider[] _results = new Collider[5];
        private int _resultsCount;

        public override void ShootFirstType()
        {
            if(!IsReadyShootFirstType())
                return;

            _resultsCount = Physics.OverlapSphereNonAlloc(transform.position, maxRadius, _results, enemyLayer);

            TryAttack();

            Debug.Log("Magic Hands Soot First");
        }

        private void TryAttack()
        {
            for (int i = 0; i < _resultsCount; i++)
            {
                if (_results[i].TryGetComponent(out IDamageable damageable))
                {
                    Vector3 directionToTarget = (_results[i].transform.position - transform.position).normalized;
                    float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
                    
                    if (angleToTarget <= coneAngle)
                    {
                        var startPointPosition = transform.position;
                        var collidersPosition = _results[i].transform.position;
                        var hasObstacle = Physics.Linecast(startPointPosition, collidersPosition, obstacleLayer);
                        
                        if (hasObstacle)
                            continue;
                        
                        damageable.TakeDamage(_damage);
                    }
                }
            }
        }

        public override void ShootSecondType()
        {
            if (!IsReadyShootSecondType())
                return;
        }

        protected override void PlayEffectsFirstType()
        {
        }

        protected override void PlayEffectsSecondType()
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