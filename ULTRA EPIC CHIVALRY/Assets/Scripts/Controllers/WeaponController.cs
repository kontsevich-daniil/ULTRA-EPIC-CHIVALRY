using System.Collections.Generic;
using System.Linq;
using Data;
using Enums;
using ScriptableObjects;
using UniRx;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class WeaponController: MonoBehaviour
    {
        private InventoryData _inventoryData;
        
        [SerializeField] private List<WeaponData> _weaponsData;
        private WeaponData _currentWeapon;

        [Inject]
        private void Initialized(InventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }
        
        private void Start()
        {
            _inventoryData.CurrentWeaponType.Subscribe(PickWeapon).AddTo(this);
        }
        
        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0)) ShootFirstType();
            if (Input.GetMouseButtonDown(1)) ShootSecondType();
                
            if (Input.GetKeyDown(KeyCode.Alpha1)) _inventoryData.SelectWeapon(EWeapon.MagicHand);
            //if (Input.GetKeyDown(KeyCode.Alpha2)) _inventoryData.SelectWeapon(1);
            //if (Input.GetKeyDown(KeyCode.Alpha3)) _inventoryData.SelectWeapon(2);
            //if (Input.GetKeyDown(KeyCode.Alpha4)) _inventoryData.SelectWeapon(3);
        }

        private void PickWeapon(EWeapon weaponType)
        {
            _currentWeapon = _weaponsData.FirstOrDefault(data => data.type == weaponType);
        }

        private void ShootFirstType()
        {
            _currentWeapon.ShootFirstType();
            Debug.Log("ShootFirst");
        }

        private void ShootSecondType()
        {
            _currentWeapon.ShootFirstType();
            Debug.Log("ShootSecond");
        }
    }
}