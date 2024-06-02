using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Levels.General;

namespace App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader
{
    public interface ILevelLoader : ICurrentLevelRestartable
    {
        List<IEntityView> Entities { get; }
        
        void LoadLevel(LevelData levelData);
    }
}