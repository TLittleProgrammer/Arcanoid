using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Levels;

namespace App.Scripts.Scenes.GameScene.Healthes
{
    public interface IHealthPointService : IAsyncInitializable<LevelData>
    {
        void UpdateHealth(int healthCount);
    }
}