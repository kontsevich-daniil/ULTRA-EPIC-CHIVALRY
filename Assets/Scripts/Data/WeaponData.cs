using System;
using Configs;
using Data.Bullet;
using Enums;
using ScriptableObjects;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Data
{
    [Serializable]
    public abstract class WeaponData: MonoBehaviour
    {
        [SerializeField] private WeaponSO weaponSo;
        
        private int _ammoCount = 0;
        protected float _cooldownFirstAttack;
        protected float _cooldownSecondAttack;
        protected ProjectileData _bulletPrefab;
        protected AudioClip _shootSound;
        
        private float lastShootTimeFirstAttack;
        private float lastShootTimeSecondAttack;
        
        private float _elapsedTime;
        
        public EWeapon type;
        public int AmmoCount => _ammoCount;

        protected void Awake()
        {
            var weaponConfig = weaponSo.GetWeapon(type);
            
            _ammoCount = weaponConfig.AmmoCount;
            _bulletPrefab = weaponConfig.BulletPrefab;
            _shootSound = weaponConfig.ShootSound;
            _cooldownFirstAttack = weaponConfig.CooldownFirstAttack;
            _cooldownSecondAttack = weaponConfig.CooldownSecondAttack;
        }

        protected bool IsReadyShootFirstType()
        {
            if(_ammoCount == 0)
                return false;
            
            if (Time.time < lastShootTimeFirstAttack + _cooldownFirstAttack) 
                return false;
            
            lastShootTimeFirstAttack = Time.time;

            if (_ammoCount >=0)
                _ammoCount--;
            
            Debug.Log("ShootFirst");
            return true;
        }

        protected bool IsReadyShootSecondType()
        {
            if (Time.time < lastShootTimeSecondAttack + _cooldownSecondAttack) 
                return false;
            
            lastShootTimeSecondAttack = Time.time;
            
            Debug.Log("ShootSecond");
            return true;
        }

        public abstract void ShootFirstType();

        public abstract void ShootSecondType();
        
        protected abstract void PlayEffectsFirstType();
        protected abstract void PlayEffectsSecondType();
    }
}