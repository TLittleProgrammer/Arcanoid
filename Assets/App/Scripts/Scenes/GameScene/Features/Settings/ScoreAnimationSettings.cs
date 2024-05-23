using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Score animation settings", fileName = "ScoreAnimationSettings")]
    public class ScoreAnimationSettings : ScriptableObject
    {
        public float Duration;
    }
}