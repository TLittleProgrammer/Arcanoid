using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Win circle animation settings", fileName = "WinCircleAnimationSettings")]
    public class CircleWinEffectSettings : ScriptableObject
    {
        public float Duration;
    }
}