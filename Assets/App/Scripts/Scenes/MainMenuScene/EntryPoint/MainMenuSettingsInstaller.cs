using App.Scripts.External.Localisation.Config;
using App.Scripts.General.Levels;
using App.Scripts.Scenes.MainMenuScene.Features.LevelPacks.Configs;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.EntryPoint
{
    [CreateAssetMenu(menuName = "Configs/Main Menu/Main Menu Settings", fileName = "MainMenuSettings")]
    public class MainMenuSettingsInstaller : ScriptableObjectInstaller
    {
        public LevelPackProvider LevelPackProvider;
        public LevelItemViewByTypeProvider LevelItemViewByTypeProvider;

        public override void InstallBindings()
        {
            Container.Bind<LevelPackProvider>().FromInstance(LevelPackProvider).IfNotBound();
            Container.Bind<LevelItemViewByTypeProvider>().FromInstance(LevelItemViewByTypeProvider).IfNotBound();
        }
    }
}