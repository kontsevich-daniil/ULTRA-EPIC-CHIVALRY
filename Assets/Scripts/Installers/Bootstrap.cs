using System.ComponentModel;
using Controllers;
using Data;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class Bootstrap: MonoInstaller
    {
        public PlayerController playerPrefab;
        public override void InstallBindings()
        {
            Container.Bind<GameController>().AsSingle().NonLazy();
            Container.Bind<LevelsController>().AsSingle().NonLazy();
            
            Container.Bind<InventoryData>().AsSingle().NonLazy();
            
            Container.Bind<PlayerController>()
                .FromComponentInNewPrefab(playerPrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}