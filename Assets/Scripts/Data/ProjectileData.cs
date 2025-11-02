   using System;
   using Data.Interfaces;
   using UnityEngine;
   using UnityEngine.UIElements;

   namespace Data.Bullet
   {
       public abstract class ProjectileData : MonoBehaviour
       {
           [Header("Common")]
           [SerializeField, Min(0f)] private float _damage = 10f;

           [Header("Rigidbody")]
           [SerializeField] private Rigidbody _projectileRigidbody;

           [Header("Effect On Destroy")]
           [SerializeField] private bool _spawnEffectOnDestroy = true;
           [SerializeField] private ParticleSystem _effectOnDestroyPrefab;
           [SerializeField, Min(0f)] private float _effectOnDestroyLifetime = 2f;

           public bool IsProjectileDisposed { get; private set; }
           public float Damage => _damage;
           public Rigidbody Rigidbody => _projectileRigidbody;

           private void OnCollisionEnter(Collision collision)
           {
               if (IsProjectileDisposed)
                   return;
            
               if (collision.gameObject.TryGetComponent(out IDamageable damageable))
               {
                   OnTargetCollision(collision, damageable);
               }
               else
               {
                   OnOtherCollision(collision);
               }
            
               OnAnyCollision(collision);
           }

           protected void DisposeProjectile()
           {
               OnProjectileDispose();
            
               SpawnEffectOnDestroy();

               Destroy(gameObject);
            
               IsProjectileDisposed = true;
           }

           private void SpawnEffectOnDestroy()
           {
               if (_spawnEffectOnDestroy == false)
                   return;
            
               var effect = Instantiate(_effectOnDestroyPrefab, transform.position, _effectOnDestroyPrefab.transform.rotation);
            
               Destroy(effect.gameObject, _effectOnDestroyLifetime);
           }

           protected virtual void OnProjectileDispose() { }
           protected virtual void OnAnyCollision(Collision collision) { }
           protected virtual void OnOtherCollision(Collision collision) { }
           protected virtual void OnTargetCollision(Collision collision, IDamageable damageable) { }
       }
   }