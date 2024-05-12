using UnityEngine;

namespace App.Scripts.General.ProjectInitialization.Settings
{
    [CreateAssetMenu(menuName = "Configs/Application/Application Settings", fileName = "ApplicationSettings")]
    public class ApplicationSettings : ScriptableObject
    {
        public int TargetFPS;
        public int VSyncCounter;
    }
}