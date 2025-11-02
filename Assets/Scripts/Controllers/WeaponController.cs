using System.Collections.Generic;
using System.Linq;
using Data;
using Enums;
using UniRx;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class WeaponController: MonoBehaviour
    {
        private InventoryData _inventoryData;
        private GameController _gameController;
        
        [SerializeField] private List<WeaponData> _weaponsData;
        private List<WeaponData> _savableWeaponsData = new();
        private WeaponData _currentWeapon;
        
        private ReactiveProperty<int> _currentAmmoCount = new(0);

        private bool _isControllerStop = false;
        public IReadOnlyReactiveProperty<int> CurrentAmmoCount => _currentAmmoCount;
        
        
        [Inject]
        private void Initialized(InventoryData inventoryData, GameController gameController)
        {
            _inventoryData = inventoryData;
            _gameController = gameController;
        }
        
        private void Start()
        {
            _inventoryData.CurrentWeaponType
                .Subscribe(PickWeapon)
                .AddTo(this);
            
            _gameController.LevelStart
                .Subscribe(_ => SaveController())
                .AddTo(this);
            SaveController();
            
            _gameController.PlayerDied.Merge(_gameController.LevelCompleted)
                .Subscribe(_ => StopController())
                .AddTo(this);
            SaveController();
            
            _gameController.LevelRestart
                .Subscribe(_ => RestartController())
                .AddTo(this);
        }
        
        private void Update()
        {
            if (_isControllerStop)
                return;
            
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButton(0)) ShootFirstType();
            if (Input.GetMouseButtonDown(1)) ShootSecondType();
                
            if (Input.GetKeyDown(KeyCode.Alpha1)) _inventoryData.SelectWeapon(EWeapon.MagicHand);
            if (Input.GetKeyDown(KeyCode.Alpha2)) _inventoryData.SelectWeapon(EWeapon.Knifes);
            if (Input.GetKeyDown(KeyCode.Alpha3)) _inventoryData.SelectWeapon(EWeapon.AK);
            if (Input.GetKeyDown(KeyCode.Alpha4)) _inventoryData.SelectWeapon(EWeapon.Crossbow);
        }

        private void PickWeapon(EWeapon weaponType)
        {
            _currentWeapon = _weaponsData.FirstOrDefault(data => data.type == weaponType);
            _currentAmmoCount.SetValueAndForceNotify(_currentWeapon.AmmoCount);
        }

        private void ShootFirstType()
        {
            _currentWeapon.ShootFirstType();
            _currentAmmoCount.SetValueAndForceNotify(_currentWeapon.AmmoCount);
        }

        private void ShootSecondType()
        {
            _currentWeapon.ShootSecondType();
            _currentAmmoCount.SetValueAndForceNotify(_currentWeapon.AmmoCount);
        }

        private void SaveController()
        {
            _savableWeaponsData.Clear();
            _savableWeaponsData.AddRange(_weaponsData);
        }

        private void StopController()
        {
            _isControllerStop = true;
        }
        
        private void RestartController()
        {
            _weaponsData.Clear();
            _weaponsData.AddRange(_savableWeaponsData);
            _inventoryData.CurrentWeaponType.Subscribe(PickWeapon).AddTo(this);
        }
    }
}