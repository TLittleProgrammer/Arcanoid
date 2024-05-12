using UnityEngine;

namespace App.Scripts.General.LoadingScreen.Settings
{
    [CreateAssetMenu(menuName = "Configs/Settings/Loading Screen Settings", fileName = "LoadingScreenSettings")]
    public class LoadingScreenSettings : ScriptableObject
    {
        public float Duration;
    }
}