using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Components;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces
{
    public interface IBoostMoveService : ITickable, IActivable, IGeneralRestartable
    {
        List<BoostView> Views { get; }
        void AddView(BoostView boostView);
        void RemoveView(BoostView boostView);
    }
}