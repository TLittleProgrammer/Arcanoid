using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Systems
{
    public interface IFireballSystem
    {
        void Activate();
        void Disable();
    }
}