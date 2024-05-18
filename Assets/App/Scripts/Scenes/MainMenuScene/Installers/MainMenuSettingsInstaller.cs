using App.Scripts.External.Localisation.Config;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Installers
{
    [CreateAssetMenu(menuName = "Configs/Main Menu/Main Menu Settings", fileName = "MainMenuSettings")]
    public class MainMenuSettingsInstaller : ScriptableObjectInstaller
    {
        public LevelPackProvider LevelPackProvider;
        public LevelItemViewByTypeProvider LevelItemViewByTypeProvider;
        public LocaleProvider LocaleProvider;

        public override void InstallBindings()
        {
            Container.Bind<LevelPackProvider>().FromInstance(LevelPackProvider).IfNotBound();
            Container.Bind<LevelItemViewByTypeProvider>().FromInstance(LevelItemViewByTypeProvider).IfNotBound();
            Container.Bind<LocaleProvider>().FromInstance(LocaleProvider).IfNotBound();
        }
    }
}