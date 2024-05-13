using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Score animation settings", fileName = "ScoreAnimationSettings")]
    public class ScoreAnimationSettings : ScriptableObject
    {
        public float Duration;
    }
}