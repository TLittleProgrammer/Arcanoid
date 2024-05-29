using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Entities.View;

namespace App.Scripts.Scenes.GameScene.Features.Levels.General.Load
{
    public interface ILevelLoader : IRestartable
    {
        List<IEntityView> Entities { get; }
        
        void LoadLevel(LevelData levelData);
    }
}