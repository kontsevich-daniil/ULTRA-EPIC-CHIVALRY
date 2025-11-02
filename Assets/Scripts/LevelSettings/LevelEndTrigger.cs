using System;
using Controllers;
using Cysharp.Threading.Tasks;
using Data;
using Installers;
using UniRx;
using UnityEngine;
using Zenject;

namespace LevelSettings
{
    public class LevelEndTrigger: MonoBehaviour
    {
        private GameController _gameController;
        [Inject]
        private void Initialized(GameController gameController)
        {
            _gameController = gameController;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            _gameController.LevelCompleted.Execute();
            Destroy(this.gameObject);
        }
    }
}