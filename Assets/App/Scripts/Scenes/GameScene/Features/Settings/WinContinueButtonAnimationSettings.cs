using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Win continue button animation settings", fileName = "WinContinueButtonAnimationSettings")]
    public class WinContinueButtonAnimationSettings : ScriptableObject
    {
        public Vector3 TargetScale;
        public float Duration;
        public float Delay;
        public Ease Ease;
    }
}