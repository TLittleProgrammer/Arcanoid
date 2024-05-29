using App.Scripts.Scenes.GameScene.Features.Levels.General;

namespace App.Scripts.Scenes.GameScene.Features.Levels.Loading
{
    public interface IMechanicsByLevelActivator
    {
        void ActivateByLevelData(LevelData levelData);
    }
}