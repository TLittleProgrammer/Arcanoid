using App.Scripts.General.Infrastructure;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.PlayerShape.Move
{
    public interface IPlayerShapeMover : ITickable, IRestartable
    {
        
    }
}