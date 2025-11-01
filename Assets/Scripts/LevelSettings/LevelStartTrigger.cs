using Controllers;
using UniRx;
using UnityEngine;
using Zenject;

namespace LevelSettings
{
    public class LevelStartTrigger: MonoBehaviour
    {
        private PlayerController _playerController;
        [Inject] DiContainer _container;
        
        [Inject]
        private void Initialized()
        {
            _playerController = _container.Resolve<PlayerController>();
        }

        private void Start()
        {
            _playerController.transform.position = transform.position;
        }
    }
}