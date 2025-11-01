using System.Collections.Generic;
using System.Linq;
using Configs;
using Data;
using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "Game/Weapon SO")]
    public class WeaponSO: ScriptableObject
    {
        public List<Configs.WeaponConfig> Weapons;

        public Configs.WeaponConfig GetWeapon(EWeapon weaponType)
        {
            return Weapons.FirstOrDefault(data => data.Type == weaponType);
        }
    }
}