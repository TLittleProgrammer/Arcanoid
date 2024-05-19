using App.Scripts.General.ProjectInitialization.Settings;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    public class ProjectInitializer : IInitializable
    {
        private readonly ApplicationSettings _applicationSettings;

        public ProjectInitializer(ApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }
        
        public void Initialize()
        {
            Application.targetFrameRate = _applicationSettings.TargetFPS;
            QualitySettings.vSyncCount = _applicationSettings.VSyncCounter;
        }
    }
}