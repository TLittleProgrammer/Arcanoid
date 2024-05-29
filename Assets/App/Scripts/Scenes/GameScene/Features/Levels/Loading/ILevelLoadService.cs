using System;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Levels.Loading
{
    public interface ILevelLoadService
    {
        event Action<LevelData> LevelLoaded;
        
        UniTask<LevelData> LoadLevel();
        UniTask<LevelData> LoadLevelNextLevel();
    }
}