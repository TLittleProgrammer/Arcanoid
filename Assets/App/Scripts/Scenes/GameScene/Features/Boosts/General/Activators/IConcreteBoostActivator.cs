﻿using System;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public interface IConcreteBoostActivator
    {
        bool IsTimeableBoost { get; }
        void Activate();
        void Deactivate();
    }
}