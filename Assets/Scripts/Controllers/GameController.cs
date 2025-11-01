using System;
using ScriptableObjects;
using UniRx;

namespace Controllers
{
    public class GameController: IDisposable
    {
        public readonly ReactiveCommand<Unit> PlayerDied = new();
        public readonly ReactiveCommand<Unit> LevelCompleted = new();

        public void Dispose()
        {
            PlayerDied?.Dispose();
            LevelCompleted?.Dispose();
        }
    }
}