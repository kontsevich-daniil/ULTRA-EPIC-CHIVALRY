using System;
using Enums;
using UnityEngine;

namespace Data.UI
{
    [Serializable]
    public struct WeaponUI
    {
        public EWeapon EWeapon;
        public GameObject WeaponObject; 
        public Animator WeaponAnimator; 
    }
}