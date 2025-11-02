using Controllers;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIPlayer : MonoBehaviour
    {
        private GameController _gameController;
        private WeaponController _weaponController;

        public TMP_Text bulletCountText;
        public GameObject deathScreen;
        public Button restartButton;

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
    }
}