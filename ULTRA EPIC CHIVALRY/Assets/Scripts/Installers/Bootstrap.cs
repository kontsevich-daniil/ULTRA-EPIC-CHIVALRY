using System.ComponentModel;
using Data;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class Bootstrap: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InventoryData>().AsSingle().NonLazy();
        }
    }
}