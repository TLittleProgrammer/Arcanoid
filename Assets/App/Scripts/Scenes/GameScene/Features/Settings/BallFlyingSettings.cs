using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Ball Flying", fileName = "BallFlying")]
    public class BallFlyingSettings : ScriptableObject
    {
        public float MaxAngle;
        public float MinAngle;
        public float Speed;
    }
}