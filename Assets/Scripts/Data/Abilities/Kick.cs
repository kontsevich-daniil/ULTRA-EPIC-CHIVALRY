using Cysharp.Threading.Tasks;
using Data.Enemy;
using Data.Interfaces;
using Enemy;
using UniRx;
using UnityEngine;

namespace Data.Abilities
{
    public class Kick: MonoBehaviour
    {
        [SerializeField] private float _damage;
        
        [Header("Cone Settings")] 
        [SerializeField] private float maxRadius;
        [Range(0, 180)] 
        [SerializeField] private float coneAngle;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask obstacleLayer;
        
        [SerializeField, Min(0f)] private float _forceAttack;
        [SerializeField, Min(0f)] private float _countdown;
        private float _elapsedTime;
        
        private Collider[] _results = new Collider[5];
        private int _resultsCount;
        
        private ReactiveCommand _kickShot = new();
        public ReactiveCommand KickShot => _kickShot;
        
        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (!(_elapsedTime >= _countdown)) 
                return;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Shoot();
                _elapsedTime = 0;
            }
        }
        
        private void Shoot()
        {
            _resultsCount = Physics.OverlapSphereNonAlloc(transform.position, maxRadius, _results, enemyLayer);
            _kickShot.Execute();
            TryAttack();
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
                        var collidersTarget = _results[i].transform;
                        var hasObstacle = Physics.Linecast(startPointPosition, collidersTarget.position, obstacleLayer);
                        
                        if (hasObstacle)
                            continue;
                        
                        damageable.TakeDamage(_damage);
                        if(_results[i].TryGetComponent(out EnemyData enemy))
                            enemy.KnockBackFrom(transform.position, _forceAttack).Forget();
                    }
                }

                if (_results[i].TryGetComponent(out IDestroyerObject damageableObject))
                {
                    damageableObject.DestroySelf();
                }
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.5f, 0.3f, 0.9f, 0.8f);
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