using System.Collections;
using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.MiniGun;

namespace App.Scripts.Scenes.GameScene.Features.PositionChecker
{
    public interface IBulletPositionChecker : IRestartable
    {
        IEnumerable<BulletView> GetAll();
        void AddBullet(BulletView bulletView);
        void RemoveBullet(BulletView bulletView);
    }
}