using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun
{
    public interface IBulletMovement
    {
        void UpdateVelocityForAll(Vector2 velocity);
        void InitializeBullet(BulletView bulletView);
        void RecalculateSpawnPositions();
        void RemoveBullet(BulletView bulletView);
    }
}