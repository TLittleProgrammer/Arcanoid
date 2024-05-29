using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Components;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move
{
    public interface IPlayerShapeMover : ITickable, IRestartable, IActivable
    {
        float Speed { get; set; }
        void ChangeSpeed(float speedScale);
    }
}