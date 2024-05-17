using App.Scripts.General.Infrastructure;

namespace App.Scripts.Scenes.GameScene.Levels.Load
{
    public interface ILevelLoader : IRestartable
    {
        void LoadLevel(LevelData levelData);
    }
}