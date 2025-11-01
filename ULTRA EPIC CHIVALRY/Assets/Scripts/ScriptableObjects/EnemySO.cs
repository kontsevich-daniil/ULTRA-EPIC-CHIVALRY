using System.Collections.Generic;
using System.Linq;
using Configs;
using Data;
using Data.Enemy;
using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "Game/Enemy SO")]
    public class EnemySO: ScriptableObject
    {
        public List<Data.Enemy.EnemyConfig> Enemyes;

        public Data.Enemy.EnemyConfig GetEnemy(EEnemy enemyType)
        {
            return Enemyes.FirstOrDefault(data => data.Type == enemyType);
        }
    }
}