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
            Container.Bind<Dictionary<string, BoostSettingsData>>().FromMethod(CreateBoostsDictionary).AsSingle().NonLazy();
        }

        private Dictionary<string, BoostSettingsData> CreateBoostsDictionary()
        {
            Dictionary<string, BoostSettingsData> result = new();

            foreach (BoostSettingsData data in _boostSettingsContainer.BoostSettingsDatas)
            {
                //var activator = DependencyInjector.CreateInstanceWithDependencies(data.ConcreteBoostActivator, Container);
                //data.ConcreteBoostActivator = activator;
                
                result.Add(data.Key, data);
            }
            
            return result;
        }
    }
}