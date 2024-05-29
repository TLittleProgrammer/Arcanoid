using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces
{
    public interface IBoostPositionChecker : ITickable
    {
        void Add(BoostView view);
        void Remove(BoostView view);
    }
}