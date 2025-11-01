using System;
using Enums;
using UnityEngine;

namespace Data.Enemy
{
    [Serializable]
    public class EnemyConfig
    {
        public EEnemy Type;
        
        public float MaxHealth = 100f;
        public float MoveSpeed = 3f;
        public float AttackDamage = 10f;
        public float DetectionRange = 10f;
        public float AttackRange = 2f;
        public float AttackCooldown = 2f;
    }
}