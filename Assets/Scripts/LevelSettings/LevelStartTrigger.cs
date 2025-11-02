using Controllers;
using Installers;
using UniRx;
using UnityEngine;
using Zenject;

namespace LevelSettings
{
    public class LevelStartTrigger: MonoBehaviour
    {
        [SerializeField] private int _currentLevelIndex;
        
        private GameController _gameController;
        private PlayerController _playerController;
        private LevelsController _levelsController;
        [Inject] DiContainer _container;
        
        [Inject]
        private void Initialized(GameController gameController)
        {
            _gameController = gameController;
            _playerController = _container.Resolve<PlayerController>();
            _levelsController = _container.Resolve<LevelsController>();
        }

        private void Start()
        {
            _gameController.LevelRestart.Merge(_gameController.LevelStart)
                .Subscribe(_ =>
                {
                    if(_currentLevelIndex == _levelsController.CurrentLevelIndex)
                        _playerController.transform.position = transform.position;
                })
                .AddTo(this);
            _playerController.transform.position = transform.position;
        }
    }
}