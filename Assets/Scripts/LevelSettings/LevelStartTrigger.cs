using Controllers;
using UniRx;
using UnityEngine;
using Zenject;

namespace LevelSettings
{
    public class LevelStartTrigger: MonoBehaviour
    {
        private GameController _gameController;
        private PlayerController _playerController;
        [Inject] DiContainer _container;
        
        [Inject]
        private void Initialized(GameController gameController)
        {
            _gameController = gameController;
            _playerController = _container.Resolve<PlayerController>();
        }

        private void Start()
        {
            _gameController.LevelRestart
                .Subscribe(_ => _playerController.transform.position = transform.position)
                .AddTo(this);
            _playerController.transform.position = transform.position;
        }
    }
}