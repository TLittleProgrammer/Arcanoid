﻿using App.Scripts.Scenes.GameScene.Features.Entities;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Activators
{
    public interface IConcreteBoostActivator
    {
        void Activate(BoostTypeId boostTypeId);
    }
}