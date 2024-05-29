using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball
{
    public interface IBallsService : ITickable
    {
        Dictionary<BallView, IBallMovementService> Balls { get; set; }
        event Action<BallView> BallAdded;
        
        void AddBall(BallView ballView, bool isFreeFlight = false);
        void UpdateSpeedByProgress(float progress);
        void Reset();
        void SetSpeedMultiplier(float multiplier);
        void SetRedBall(bool activated);
        void SetSticky(BallView view);
        void Fly(BallView view);
    }
}