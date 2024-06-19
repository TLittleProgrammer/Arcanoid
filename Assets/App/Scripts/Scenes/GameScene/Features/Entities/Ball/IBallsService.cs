using System;
using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball
{
    public interface IBallsService : ITickable, IGeneralRestartable
    {
        List<BallView> Balls { get; }
        event Action<BallView> BallAdded;
        
        void AddBall(BallView ballView, bool isFreeFlight = false);
        void UpdateSpeedByProgress(float progress);
        void SetSpeedMultiplier(float multiplier);
        void SetRedBall(bool activated);
        void SetSticky(BallView view);
        void Fly(BallView view);
        void DespawnAll();
        bool BallIsSticked(BallView ballView);
    }
}