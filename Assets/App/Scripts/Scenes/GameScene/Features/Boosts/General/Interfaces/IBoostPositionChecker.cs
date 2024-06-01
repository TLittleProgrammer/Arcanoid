using App.Scripts.General.Infrastructure;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces
{
    public interface IBoostPositionChecker : ITickable, IGeneralRestartable
    {
        void Add(BoostView view);
        void Remove(BoostView view);
    }
}