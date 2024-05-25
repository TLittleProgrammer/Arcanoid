using App.Scripts.Scenes.GameScene.Features.Entities;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Helpers
{
    public interface IConcreteBoostActivator
    {
        void Activate(BoostTypeId boostTypeId);
    }
}