﻿using App.Scripts.General.Infrastructure;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Ball.Movement
{
    public interface IBallMovementService : ITickable, IRestartable
    {
        void UpdateSpeed(float addValue);
    }
}