﻿using App.Scripts.Scenes.GameScene.Features.Components;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces
{
    public interface IBoostMoveService : ITickable, IActivable
    {
        void AddView(BoostView boostView);
        void RemoveView(BoostView boostView);
    }
}