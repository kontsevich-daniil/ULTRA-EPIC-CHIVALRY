using Configs;
using Data.Interfaces;
using UnityEngine;

namespace Data.Weapons
{
    public class AK: WeaponData
    {
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private Transform orientationTransform;
        
        [SerializeField] private Transform cameraTransform;
        
        [SerializeField] private Transform muzzle;
        [SerializeField, Min(0f)] private float _force = 35f;
        
        [SerializeField, Min(0f)] private float _forceDashFirstAttck = 35f;
        [SerializeField, Min(0f)] private float _forceDashSecondAttack = 35f;
        
        public override void ShootFirstType()
        {
            if(!IsReadyShootFirstType())
                return;
            
            if (_bulletPrefab != null)
            {
                var projectile = Instantiate(_bulletPrefab, muzzle.position, muzzle.rotation);
                projectile.Rigidbody.AddForce(muzzle.forward * _force, ForceMode.Impulse);
                playerRigidbody.AddForce(orientationTransform.forward * _forceDashFirstAttck, ForceMode.Impulse);
            }
            
        }

        public override void ShootSecondType()
        {
            if (!IsReadyShootSecondType())
                return;
            
            playerRigidbody.AddForce(cameraTransform.forward * _forceDashSecondAttack, ForceMode.Impulse);
        }

        protected override void PlayEffectsFirstType()
        {
            /*if (shootSound != null)
            {
                AudioSource.PlayClipAtPoint(shootSound, shootOrigin.position);
            }*/
        }

        protected override void PlayEffectsSecondType()
        {
            
        }
    }
}