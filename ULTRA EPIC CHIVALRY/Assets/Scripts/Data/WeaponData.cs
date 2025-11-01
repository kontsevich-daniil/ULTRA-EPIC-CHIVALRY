using System;
using Configs;
using Enums;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Data
{
    [Serializable]
    public abstract class WeaponData: MonoBehaviour
    {
        public WeaponSettings weaponSettings;
        
        protected float Damage;
        protected float FireRate;
        protected int AmmoCount;
        protected GameObject BulletPrefab;
        protected AudioClip ShootSound;
        protected float CooldownFirstAttack;
        protected float CooldownSecondAttack;
        
        public EWeapon type;

        protected void Awake()
        {
            var weaponConfig = weaponSettings.GetWeapon(type);
            
            Damage = weaponConfig.Damage;
            FireRate = weaponConfig.FireRate;
            AmmoCount = weaponConfig.AmmoCount;
            BulletPrefab = weaponConfig.BulletPrefab;
            ShootSound = weaponConfig.ShootSound;
            CooldownFirstAttack = weaponConfig.CooldownFirstAttack;
            CooldownSecondAttack = weaponConfig.CooldownSecondAttack;
        }

        
        public abstract void ShootFirstType();
        
        public abstract void ShootSecondType();
        protected abstract void PlaySound();

        protected abstract void EnableVFX();
    }
}