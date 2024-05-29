using System;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Entities;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces
{
    public interface IBoostContainer : ITickable, IActivable, IRestartable
    {
        event Action<BoostTypeId> BoostEnded;
        void AddActive(BoostTypeId boostTypeId);
    }
}