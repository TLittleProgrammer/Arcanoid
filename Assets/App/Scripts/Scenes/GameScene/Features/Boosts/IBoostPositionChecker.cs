using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts
{
    public interface IBoostPositionChecker : ITickable
    {
        void Add(BoostView view);
    }
}