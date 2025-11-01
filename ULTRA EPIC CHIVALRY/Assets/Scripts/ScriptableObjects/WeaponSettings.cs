using System.Collections.Generic;
using System.Linq;
using Configs;
using Data;
using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponSettings", menuName = "Game/Weapon Settings")]
    public class WeaponSettings: ScriptableObject
    {
        public List<WeaponConfig> Weapons;

        public WeaponConfig GetWeapon(EWeapon weaponType)
        {
            return Weapons.FirstOrDefault(data => data.Type == weaponType);
        }
    }
}