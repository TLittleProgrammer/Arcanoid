using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game/BirdSettings", fileName = "BirdSettings")]
    public class BirdSettings : ScriptableObject
    {
        public float Amplitude = 1f;
        public float HorizontalSpeed = 2f;
        public float RespawnTime = 2f;
        public int InitialHealthCount = 1;
    }
}