using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;

namespace App.Scripts.Scenes.GameScene.Features.Ball
{
    public interface IBallsService
    {
        Dictionary<BallView, IBallMovementService> Balls { get; set; }
        event Action<BallView> BallAdded;
        float SpeedMultiplier { get; }

        void AddBall(BallView ballView, bool isFreeFlight = false);
        void UpdateSpeedByProgress(float progress);
        void Reset();
        void SetSpeedMultiplier(float multiplier);
        void SetRedBall(bool activated);
        void SetSticky(BallView view);
        void Fly(BallView view);
    }
}