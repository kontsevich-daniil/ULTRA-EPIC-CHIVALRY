using System;
using System.Collections.Generic;
using Enums;
using UniRx;

namespace Data
{
    public sealed class InventoryData: IDisposable
    {
        private List<WeaponData> _weapons;
    
        private ReactiveProperty<EWeapon> _currentWeaponType = new(EWeapon.MagicHand);
        public IReadOnlyReactiveProperty<EWeapon> CurrentWeaponType => _currentWeaponType;
    
        public void SelectWeapon(EWeapon eWeapon)
        {
            _currentWeaponType.Value = eWeapon;
        }

        public void Dispose()
        {
            _currentWeaponType.Dispose();
        }
    }
}