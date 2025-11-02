using System.Collections.Generic;
using System.Linq;
using Controllers;
using Data;
using Data.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIPlayer : MonoBehaviour
    {
        private GameController _gameController;
        private WeaponController _weaponController;

        private WeaponUI _weaponUI;

        public TMP_Text bulletCountText;
        public GameObject deathScreen;
        public Button restartButton;
        
        public WeaponUI legUI;
        public List<WeaponUI> weaponsUI;

        [Inject]
        private void Initialized(GameController gameController, WeaponController weaponController)
        {
            _gameController = gameController;
            _weaponController = weaponController;
        }

        private void Start()
        {
            _gameController.PlayerDied
                .Subscribe(_ => deathScreen.SetActive(true))
                .AddTo(this);
            
            _gameController.LevelRestart
                .Subscribe(_ => deathScreen.SetActive(false))
                .AddTo(this);

            _weaponController.CurrentAmmoCount
                .Subscribe(SetBulletText)
                .AddTo(this);

            _weaponController.CurrentWeaponData
                .Subscribe(SetRightHandImage)
                .AddTo(this);
            
            _weaponController.WeaponFirstTypeAttack
                .Subscribe(_ => PlayWeaponAnimation())
                .AddTo(this);
            
            _weaponController.Kick
                .Subscribe(_ => PlayKickAnimation())
                .AddTo(this);

            restartButton.onClick
                .AsObservable()
                .Subscribe(_ => _gameController.LevelRestart.Execute())
                .AddTo(this);
        }

        private void SetBulletText(int count)
        {
            if (count < 0)
            {
                bulletCountText.enabled = false;
                return;
            }

            bulletCountText.enabled = true;
            bulletCountText.text = count.ToString();
        }

        private void SetRightHandImage(WeaponData data)
        {
            _weaponUI.WeaponObject?.SetActive(false);
            _weaponUI = weaponsUI.First(weaponUI => weaponUI.EWeapon.Equals(data.type));
            _weaponUI.WeaponObject.SetActive(true);
        }

        private void PlayWeaponAnimation()
        {
            _weaponUI.WeaponAnimator.Play("Attack");
        }

        private void PlayKickAnimation()
        {
            legUI.WeaponAnimator.Play("Attack");
        }
    }
}