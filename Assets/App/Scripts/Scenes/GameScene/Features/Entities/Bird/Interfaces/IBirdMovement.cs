﻿using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces
{
    public interface IBirdMovement : ITickable, IActivable
    {
        Direction Direction { get; set; }
    }
}