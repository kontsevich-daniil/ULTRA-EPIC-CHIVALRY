using System;
using Data.Bullet;
using Enums;
using UnityEngine;

namespace Configs
{
    [Serializable]
    public class WeaponConfig
    {
        [Header("Data")]
        public EWeapon Type;
        public int AmmoCount;
        public ProjectileData BulletPrefab;
        public float CooldownFirstAttack;
        public float CooldownSecondAttack;
        
        [Header("Audio")]
        public AudioClip ShootSound;
    }
}