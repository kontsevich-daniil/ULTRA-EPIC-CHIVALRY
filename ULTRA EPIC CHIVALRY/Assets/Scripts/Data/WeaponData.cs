using System;
using Configs;
using Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Data
{
    [Serializable]
    public abstract class WeaponData: MonoBehaviour
    {
        [FormerlySerializedAs("weaponConfig")] [FormerlySerializedAs("weaponSettings")] public WeaponSO weaponSo;
        
        protected float _damage;
        protected int _ammoCount;
        protected float _cooldownFirstAttack;
        protected float _cooldownSecondAttack;
        protected GameObject _bulletPrefab;
        protected AudioClip _shootSound;
        
        public EWeapon type;

        protected void Awake()
        {
            var weaponConfig = this.weaponSo.GetWeapon(type);
            
            _damage = weaponConfig.Damage;
            _ammoCount = weaponConfig.AmmoCount;
            _bulletPrefab = weaponConfig.BulletPrefab;
            _shootSound = weaponConfig.ShootSound;
            _cooldownFirstAttack = weaponConfig.CooldownFirstAttack;
            _cooldownSecondAttack = weaponConfig.CooldownSecondAttack;
        }
        
        public abstract void ShootFirstType();
        public abstract void ShootSecondType();
        protected abstract void PlaySound();
        protected abstract void EnableVFX();
    }
}