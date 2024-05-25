using System;
using App.Scripts.Scenes.GameScene.Features.Entities;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces
{
    public interface IBoostContainer : ITickable
    {
        event Action<BoostTypeId> BoostEnded;
        void AddActive(BoostTypeId boostTypeId);
    }
}