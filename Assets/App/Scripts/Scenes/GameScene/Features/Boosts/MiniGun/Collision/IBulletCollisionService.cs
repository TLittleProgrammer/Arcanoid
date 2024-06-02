using System;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Collision
{
    public interface IBulletCollisionService
    {
        event Action<BulletView, EntityView> BulletWasCollidired;
        void AddBullet(BulletView bulletView);
        void OnBulletCollided(BulletView bulletView, Collision2D collision2D);
    }
}