using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Entities;

namespace App.Scripts.Scenes.GameScene.Levels.Load
{
    public interface ILevelLoader : IRestartable
    {
        List<IEntityView> Entities { get; }
        
        void LoadLevel(LevelData levelData);
    }
}