using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Stop Game animation settings", fileName = "StopGameAnimationSettings")]
    public class StopGameSettings : ScriptableObject
    {
        public float TimeScaleChangeDuration;
    }
}