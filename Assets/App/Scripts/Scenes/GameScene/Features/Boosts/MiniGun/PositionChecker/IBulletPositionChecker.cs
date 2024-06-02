using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.PositionChecker
{
    public interface IBulletPositionChecker : IGeneralRestartable
    {
        IEnumerable<BulletView> GetAll();
        void AddBullet(BulletView bulletView);
        void RemoveBullet(BulletView bulletView);
    }
}