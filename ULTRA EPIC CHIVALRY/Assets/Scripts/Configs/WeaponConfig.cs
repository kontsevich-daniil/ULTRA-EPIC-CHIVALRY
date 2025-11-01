using System;
using Enums;
using UnityEngine;

namespace Configs
{
    [Serializable]
    public class WeaponConfig
    {
        [Header("Data")]
        public EWeapon Type;
        public float Damage;
        public float FireRate;
        public int AmmoCount;
        public GameObject BulletPrefab;
        public float CooldownFirstAttack;
        public float CooldownSecondAttack;
        
        [Header("Audio")]
        public AudioClip ShootSound;
    }
}