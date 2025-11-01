using Controllers;
using Data;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PlayerInstaller: MonoInstaller
    {
        public WeaponController weaponController;
        
        public override void InstallBindings()
        {
            Container.Bind<WeaponController>().FromInstance(weaponController).AsSingle();
        }
    }
}