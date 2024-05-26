﻿using System.Collections;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.MiniGun;

namespace App.Scripts.Scenes.GameScene.Features.PositionChecker
{
    public interface IBulletPositionChecker
    {
        IEnumerable<BulletView> GetAll();
        void AddBullet(BulletView bulletView);
        void RemoveBullet(BulletView bulletView);
    }
}