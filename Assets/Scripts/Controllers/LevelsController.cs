using System;
using Controllers;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;

namespace Installers
{
    public class LevelsController: IDisposable
    {
        private GameController _gameController;

        private bool _isLoaded;
        private bool _shouldLoad;
        private int _maxLevelCount = 1;
        private int _currentLevelIndex = 0;

        private CompositeDisposable levelsDisposable = new();
        public int CurrentLevelIndex => _currentLevelIndex;

        [Inject]
        private void Initialized(GameController gameController)
        {
            _gameController = gameController;
            Subscribe();
        }

        private void Subscribe()
        {
            _gameController.LevelStart
                .Subscribe(_ => NextLevel())
                .AddTo(levelsDisposable);
            
            _gameController.LevelRestart
                .Subscribe(_ => RestartLevel())
                .AddTo(levelsDisposable);
        }

        private void LoadScene(int levelIndex)
        {
            if (_isLoaded)
                return;

            SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
            _isLoaded = true;
        }

        private void UnLoadScene()
        {
            /*if (!_isLoaded)
                return;*/

            SceneManager.UnloadSceneAsync(_currentLevelIndex - 1);
            _isLoaded = false;
        }

        private void NextLevel()
        {
            _currentLevelIndex++;
            LoadScene(_currentLevelIndex);
            UnLoadScene();
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(_currentLevelIndex);
        }

        public void NewGame()
        {
            LoadScene(_currentLevelIndex);
        }

        public void Dispose()
        {
            levelsDisposable?.Dispose();
        }
    }
}