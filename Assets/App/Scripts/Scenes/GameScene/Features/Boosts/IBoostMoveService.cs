using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts
{
    public interface IBoostMoveService : ITickable
    {
        void AddView(BoostView boostView);
        void RemoveView(BoostView boostView);
    }
}