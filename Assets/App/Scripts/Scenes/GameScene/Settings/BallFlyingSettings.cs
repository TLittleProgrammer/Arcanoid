using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Ball Flying", fileName = "BallFlying")]
    public class BallFlyingSettings : ScriptableObject
    {
        public float MaxAngle;
        public float MinAngle;
        public float Speed;
    }
}