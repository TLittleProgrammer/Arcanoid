using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game/PLayerMovingSettings", fileName = "PlayerMovingSettings")]
    public class ShapeMoverSettings : ScriptableObject
    {
        public float Speed = 5f;
    }
}