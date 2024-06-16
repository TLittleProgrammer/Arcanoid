using System;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Components;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces
{
    public interface IBoostContainer : ITickable, IActivable, IGeneralRestartable
    {
        event Action<string> BoostEnded;
        event Action<string> DeactivateBoost;
        void AddBoost(string boostTypeId);
        bool BoostIsActive(string boostTypeId);
    }
}