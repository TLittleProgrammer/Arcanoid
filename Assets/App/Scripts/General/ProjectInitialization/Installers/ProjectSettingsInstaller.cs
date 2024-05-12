using App.Scripts.General.LoadingScreen.Settings;
using App.Scripts.General.ProjectInitialization.Settings;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    [CreateAssetMenu(menuName = "Installers/Project Settings", fileName = "ProjectSettings")]
    public class ProjectSettingsInstaller : ScriptableObjectInstaller
    {
        public ApplicationSettings ApplicationSettings;
        public LoadingScreenSettings LoadingScreenSettings;

        public override void InstallBindings()
        {
            Container.Bind<ApplicationSettings>().FromInstance(ApplicationSettings).IfNotBound();
            Container.Bind<LoadingScreenSettings>().FromInstance(LoadingScreenSettings).IfNotBound();
        }
    }
}