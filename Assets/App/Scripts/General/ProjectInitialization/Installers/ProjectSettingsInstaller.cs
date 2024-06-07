using App.Scripts.External.Localisation.Config;
using App.Scripts.General.Energy;
using App.Scripts.General.Levels;
using App.Scripts.General.LoadingScreen.Settings;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.General.ProjectInitialization.Settings;
using App.Scripts.General.Providers;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    [CreateAssetMenu(menuName = "Installers/Project Settings", fileName = "ProjectSettings")]
    public class ProjectSettingsInstaller : ScriptableObjectInstaller
    {
        public ApplicationSettings ApplicationSettings;
        public LoadingScreenSettings LoadingScreenSettings;
        public LevelPackProvider LevelPackProvider;
        public EnergySettings EnergySettings;
        public SpriteProvider SpriteProvider;
        public LocaleProvider LocaleProvider;
        public ViewPopupMapping ViewPopupMapping;

        public override void InstallBindings()
        {
            Container.Bind<ApplicationSettings>().FromInstance(ApplicationSettings).IfNotBound();
            Container.Bind<LoadingScreenSettings>().FromInstance(LoadingScreenSettings).IfNotBound();
            Container.Bind<LevelPackProvider>().FromInstance(LevelPackProvider).IfNotBound();
            Container.Bind<EnergySettings>().FromInstance(EnergySettings).IfNotBound();
            Container.Bind<SpriteProvider>().FromInstance(SpriteProvider).IfNotBound();
            Container.Bind<LocaleProvider>().FromInstance(LocaleProvider).IfNotBound();
            Container.Bind<ViewPopupMapping>().FromInstance(ViewPopupMapping).IfNotBound();
        }
    }
}