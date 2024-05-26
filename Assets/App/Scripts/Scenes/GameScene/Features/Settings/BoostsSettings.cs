using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game/BoostsSettings", fileName = "BoostsSettings")]
    public class BoostsSettings : SerializedScriptableObject
    {
        [BoxGroup("Ball speed")]
        public float BallSpeedDuration;
        [BoxGroup("Ball speed")]
        public float SlowDownPercentFromAll;
        [BoxGroup("Ball speed")]
        public float AcceleratePercentFromAll;
        
        [BoxGroup("Player Shape size")]
        public float ShapeSizeDuration;
        [BoxGroup("Player Shape size")]
        public float AddPercent;
        [BoxGroup("Player Shape size")]
        public float MinusPercent;
        public Dictionary<BoostTypeId, Sprite> PlayerShapeSprites;
        
        [BoxGroup("Player Shape speed")]
        public float ShapeSpeedDuration;
        [BoxGroup("Player Shape speed")]
        public float AddPercentSpeed;
        [BoxGroup("Player Shape speed")]
        public float MinusPercentSpeed;
        
        [BoxGroup("Health")]
        public int AddHealth;
        [BoxGroup("Health")]
        public int MinusHealth;
        
        [BoxGroup("Fireeball")]
        public float FireballDuration;
        
        
        [BoxGroup("Sticly")]
        public float StickyDuration = 5f;
        
        [BoxGroup("MiniGun")]
        public float MiniGunDuration;
        [BoxGroup("MiniGun")]
        public float BulletTimeOffset = 0.25f;
        [BoxGroup("MiniGun")]
        public int BulletDamage = 1;
        [BoxGroup("MiniGun")]
        public float BulletSpeed = 1f;
    }
}