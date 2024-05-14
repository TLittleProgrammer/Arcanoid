using App.Scripts.Scenes.GameScene.Infrastructure;
using Zenject;

namespace App.Scripts.Scenes.GameScene.PlayerShape.Move
{
    public interface IPlayerShapeMover : ITickable, IRestartable
    {
        
    }
}