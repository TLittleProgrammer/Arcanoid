using App.Scripts.General.Infrastructure;
using Zenject;

namespace App.Scripts.Scenes.GameScene.PlayerShape.Move
{
    public interface IPlayerShapeMover : ITickable, IRestartable
    {
        
    }
}