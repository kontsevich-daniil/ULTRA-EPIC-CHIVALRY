using Configs;
using Data.Interfaces;
using UnityEngine;

namespace Data.Weapons
{
    public class Crossbow : WeaponData
    {
        [SerializeField] private Transform muzzle;
        [SerializeField, Min(0f)] private float _force = 35f;
        
        public override bool ShootFirstType()
        {
            if(!IsReadyShootFirstType())
                return false;
            
            if (_bulletPrefab != null)
            {
                var projectile = Instantiate(_bulletPrefab, muzzle.position, muzzle.rotation);
                projectile.Rigidbody.AddForce(muzzle.forward * _force, ForceMode.Impulse);
            }
            return true;
        }

        public override void ShootSecondType()
        {
            if (!IsReadyShootSecondType())
                return;
            Destroy(this.gameObject);
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