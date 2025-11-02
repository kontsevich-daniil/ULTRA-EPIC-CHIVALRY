using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ScriptableObjects;
using UniRx;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class GameController : IDisposable
    {
        public readonly ReactiveCommand PlayerDied = new();
        public readonly ReactiveCommand LevelCompleted = new();
        public readonly ReactiveCommand LevelStart = new();
        public readonly ReactiveCommand LevelRestart = new();
        private ReactiveProperty<int> _timer = new(60);
        public ReactiveProperty<int> Timer => _timer;

        private CancellationTokenSource _cancellationToken = new();

        [Inject]
        private void Initialized(GameController gameController)
        {
            StartTimer(_cancellationToken.Token).Forget();
            LevelStart
                .Merge(LevelRestart)
                .Subscribe(_ => { ResetTimer(); });
        }

        private async UniTaskVoid StartTimer(CancellationToken token)
        {
            var timer = _timer.Value;
            for (int i = 0; i < timer; i++)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                _timer.Value--;

                if (_timer.Value == 0)
                {
                    PlayerDied.Execute();
                }
            }
        }

        private void ResetTimer()
        {
            _cancellationToken.Cancel();
            _cancellationToken = new CancellationTokenSource();

            _timer.Value = 60;
            StartTimer(_cancellationToken.Token).Forget();
        }

        public void Dispose()
        {
            PlayerDied?.Dispose();
            LevelCompleted?.Dispose();
        }
    }
}