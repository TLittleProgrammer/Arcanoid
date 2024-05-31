using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun;

namespace App.Scripts.Scenes.GameScene.Features.PositionChecker
{
    public interface IBulletPositionChecker : IGeneralRestartable
    {
        IEnumerable<BulletView> GetAll();
        void AddBullet(BulletView bulletView);
        void RemoveBullet(BulletView bulletView);
    }
}