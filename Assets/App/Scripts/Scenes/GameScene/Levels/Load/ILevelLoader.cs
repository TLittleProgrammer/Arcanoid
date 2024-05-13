using App.Scripts.Scenes.GameScene.Infrastructure;

namespace App.Scripts.Scenes.GameScene.Levels.Load
{
    public interface ILevelLoader : IRestartable
    {
        void LoadLevel(LevelData levelData);
    }
}