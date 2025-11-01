using Configs;
using Data.Interfaces;
using UnityEngine;

namespace Data.Weapons
{
    public class Knifes: WeaponData
    {
        [SerializeField] private Transform muzzle;
        [SerializeField, Min(0f)] private float _force = 35f;
        
        public override void ShootFirstType()
        {
            if(!IsReadyShootFirstType())
                return;
            
            if (_bulletPrefab != null)
            {
                var projectile = Instantiate(_bulletPrefab, muzzle.position, muzzle.rotation);
                projectile.Rigidbody.AddForce(muzzle.forward * _force, ForceMode.Impulse);
            }
            
        }

        public override void ShootSecondType()
        {
            if (!IsReadyShootSecondType())
                return; 
            
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