using App.Scripts.External.Initialization;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Levels.General;

namespace App.Scripts.Scenes.GameScene.Features.Healthes
{
    public interface IViewHealthPointService : IAsyncInitializable<LevelData>, IRestartable
    {
        void UpdateHealth(int healthCount);
    }
}