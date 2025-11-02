using System.ComponentModel;
using Controllers;
using Data;
using ScriptableObjects;
using Sounds;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class Bootstrap: MonoInstaller
    {
        public PlayerData playerPrefab;
        public SoundController soundController;
        public UIBase UIBase;
        public SoundController SoundController;
        public override void InstallBindings()
        {
            Container.Bind<GameController>().AsSingle().NonLazy();
            Container.Bind<LevelsController>().AsSingle().NonLazy();
            
            Container.Bind<InventoryData>().AsSingle().NonLazy();

            var instance = Container.InstantiatePrefab(playerPrefab);
            
            Container.Bind<PlayerController>()
                .FromInstance(instance.GetComponent<PlayerData>().playerController)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<PlayerMovement>()
                .FromInstance(instance.GetComponent<PlayerData>().playerMovement)
                .AsSingle();
            
            Container.Bind<WeaponController>()
                .FromInstance(instance.GetComponent<PlayerData>().weaponController)
                .AsSingle();
            
            Container.Bind<SoundController>()
                .FromComponentsInNewPrefab(soundController)
                .AsSingle()
                .NonLazy();
            
            var instanceUI = Container.InstantiatePrefab(UIBase);
            
            Container.Bind<UIPlayer>()
                .FromInstance(instanceUI.GetComponent<UIPlayer>())
                .AsSingle();
            
        }
    }
}