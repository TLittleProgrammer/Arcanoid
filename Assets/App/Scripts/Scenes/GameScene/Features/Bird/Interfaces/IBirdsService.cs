﻿using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Components;

namespace App.Scripts.Scenes.GameScene.Features.Bird
{
    public interface IBirdsService : IRestartable, IActivable
    {
        Dictionary<BirdView, IBirdMovement> Birds { get; }
        void AddBird(BirdView birdView);
        void GoFly(BirdView birdView);
        void Destroy(BirdView birdView);
        void StopAll();
    }
}