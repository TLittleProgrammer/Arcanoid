using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game/BoostsSettings", fileName = "BoostsSettings")]
    public class BoostsSettings : ScriptableObject
    {
        [BoxGroup("Ball speed")]
        public float BallSpeedDuration;
        [BoxGroup("Ball speed")]
        public float SlowDownPercentFromAll;
        [BoxGroup("Ball speed")]
        public float AcceleratePercentFromAll;
    }
}