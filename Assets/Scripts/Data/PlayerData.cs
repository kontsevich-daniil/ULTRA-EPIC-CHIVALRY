using System;
using Controllers;
using UniRx;
using UnityEngine;
using Zenject;

namespace Data
{
    public class PlayerData : MonoBehaviour
    {
        private GameController _gameController;

        public PlayerController playerController;
        public PlayerMovement playerMovement;
        public PlayerCameraController playerCameraController;
        public WeaponController weaponController;

        [Inject]
        private void Initialized(GameController gameController)
        {
            _gameController = gameController;
        }

        private void Awake()
        {
            _gameController.LevelCompleted
                .Merge(_gameController.PlayerDied)
                .Subscribe(_ => StopControllers())
                .AddTo(this);
            
            _gameController.LevelStart
                .Subscribe(_ => StartControllers())
                .AddTo(this);
            
            _gameController.LevelRestart
                .Subscribe(_ => RestartControllers())
                .AddTo(this);
        }

        private void StopControllers()
        {
            playerMovement.StopController();
            playerCameraController.StopController();
        }

        private void StartControllers()
        {
            playerMovement.StartController();
            playerCameraController.StartController();
        }

        private void RestartControllers()
        {
            playerController.ResetPlayer();
            playerMovement.StartController();
            playerCameraController.StartController();
        }
    }
}