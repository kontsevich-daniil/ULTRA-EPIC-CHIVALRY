using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Abilities;
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
        
        [SerializeField] private Kick _kick;
        [SerializeField] private List<WeaponData> _weaponsData;
        private List<WeaponData> _savableWeaponsData = new();
        private ReactiveProperty<WeaponData> _currentWeapon = new();
        
        private ReactiveProperty<int> _currentAmmoCount = new(0);

        private bool _isControllerStop = false;
        public IReadOnlyReactiveProperty<WeaponData> CurrentWeaponData => _currentWeapon;
        public IReadOnlyReactiveProperty<int> CurrentAmmoCount => _currentAmmoCount;
        public ReactiveCommand Kick = new();
        
        public ReactiveCommand WeaponFirstTypeAttack = new();
        public ReactiveCommand WeaponSecondTypeAttack = new();
        
        [Inject]
        private void Initialized(InventoryData inventoryData, GameController gameController)
        {
            _inventoryData = inventoryData;
            _gameController = gameController;
        }
        
        private void Start()
        {
            Kick = _kick.KickShot;
            
            _inventoryData.CurrentWeaponType
                .Subscribe(PickWeapon)
                .AddTo(this);
            
            _gameController.LevelStart
                .Subscribe(_ =>
                {
                    RestartController();
                    SaveController();
                })
                .AddTo(this);
            
            _gameController.PlayerDied.Merge(_gameController.LevelCompleted)
                .Subscribe(_ => StopController())
                .AddTo(this);
            
            _gameController.LevelRestart
                .Subscribe(_ => RestartController())
                .AddTo(this);
            
            SaveController();
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
            _currentWeapon.Value = _weaponsData.FirstOrDefault(data => data.type == weaponType);
            _currentAmmoCount.SetValueAndForceNotify(_currentWeapon.Value.AmmoCount);
        }

        private void ShootFirstType()
        {
            if (_currentWeapon.Value.ShootFirstType())
                WeaponFirstTypeAttack.Execute();
            
            _currentAmmoCount.SetValueAndForceNotify(_currentWeapon.Value.AmmoCount);
        }

        private void ShootSecondType()
        {
            WeaponSecondTypeAttack.Execute();
            _currentWeapon.Value.ShootSecondType();
            _currentAmmoCount.SetValueAndForceNotify(_currentWeapon.Value.AmmoCount);
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
            _isControllerStop = false;
            _weaponsData.Clear();
            _weaponsData.AddRange(_savableWeaponsData);
            _inventoryData.CurrentWeaponType.Subscribe(PickWeapon).AddTo(this);
        }
    }
}