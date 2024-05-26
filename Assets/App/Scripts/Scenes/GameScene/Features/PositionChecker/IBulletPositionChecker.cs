using App.Scripts.Scenes.GameScene.Features.MiniGun;

namespace App.Scripts.Scenes.GameScene.Features.PositionChecker
{
    public interface IBulletPositionChecker
    {
        void AddBullet(BulletView bulletView);
        void RemoveBullet(BulletView bulletView);
    }
}