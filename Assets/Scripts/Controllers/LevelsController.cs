using System;
using Controllers;
using Cysharp.Threading.Tasks;
using Data;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;

namespace Installers
{
    public class LevelsController: IDisposable
    {
        private GameController _gameController;
        private PlayerData _playerData;

        private bool _shouldLoad;
        private int _maxLevelCount = 1;
        private int _currentLevelIndex = 0;

        private CompositeDisposable levelsDisposable = new();
        public int CurrentLevelIndex => _currentLevelIndex;

        [Inject]
        private void Initialized(GameController gameController, PlayerData playerData)
        {
            _gameController = gameController;
            _playerData = playerData;
            Subscribe();
        }

        private void Subscribe()
        {
            _gameController.LevelCompleted
                .Subscribe(_ => NextLevel().Forget())
                .AddTo(levelsDisposable);
            
            _gameController.LevelRestart.Subscribe(async _ =>
            {
                await RestartLevel();
                _playerData.RestartControllers();
            }).AddTo(levelsDisposable);
        }

        private async UniTask LoadScene(int levelIndex)
        {
            await SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
        }

        private void UnLoadScene()
        {
            SceneManager.UnloadSceneAsync(_currentLevelIndex - 1);
        }

        private async UniTask NextLevel()
        {
            _currentLevelIndex++;
            await LoadScene(_currentLevelIndex);
            UnLoadScene();
        }

        private async UniTask RestartLevel()
        {
            await SceneManager.LoadSceneAsync(_currentLevelIndex, LoadSceneMode.Single);
        }

        public void NewGame()
        {
            LoadScene(_currentLevelIndex).Forget();
        }

        public void Dispose()
        {
            levelsDisposable?.Dispose();
        }
    }
}