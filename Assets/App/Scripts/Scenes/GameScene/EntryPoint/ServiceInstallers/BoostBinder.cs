using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class BoostBinder : Installer<BoostSettingsContainer, BoostBinder>
    {
        private readonly BoostSettingsContainer _boostSettingsContainer;

        public BoostBinder(BoostSettingsContainer boostSettingsContainer)
        {
            _boostSettingsContainer = boostSettingsContainer;
        }
        
        public override void InstallBindings()
        {
            Container.Bind<Dictionary<string, IConcreteBoostActivator>>().FromMethod(CreateBoostsDictionary).AsSingle().NonLazy();
        }

        private Dictionary<string, IConcreteBoostActivator> CreateBoostsDictionary()
        {
            Dictionary<string, IConcreteBoostActivator> result = new();

            foreach (BoostSettingsData data in _boostSettingsContainer.BoostSettingsDatas)
            {
                var activator = DependencyInjector.CreateInstanceWithDependencies(data.ConcreteBoostActivator, Container);
                
                result.Add(data.Key, activator);
            }
            
            return result;
        }
    }
}