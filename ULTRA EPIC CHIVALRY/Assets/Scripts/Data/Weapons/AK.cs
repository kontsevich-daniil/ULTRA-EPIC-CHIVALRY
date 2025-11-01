using Configs;
using Data.Interfaces;

namespace Data.Weapons
{
    public class AK: WeaponData
    {
        public override void ShootFirstType()
        {
            /*if (Time.time < nextFireTime) return;

            nextFireTime = Time.time + FireRate;

            if (BulletPrefab != null)
            {
                GameObject bullet = Instantiate(BulletPrefab, shootOrigin.position, shootOrigin.rotation);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddForce(shootOrigin.forward * 1000f, ForceMode.Impulse);
            }

            if (shootSound != null)
            {
                AudioSource.PlayClipAtPoint(shootSound, shootOrigin.position);
            }*/
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
    }
}