using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game/BoostsSettings", fileName = "BoostsSettings")]
    public class BoostsSettings : SerializedScriptableObject
    {
        [BoxGroup("MiniGun")]
        public float BulletTimeOffset = 0.25f;
        [BoxGroup("MiniGun")]
        public int BulletDamage = 1;
        [BoxGroup("MiniGun")]
        public float BulletSpeed = 1f;
    }
}