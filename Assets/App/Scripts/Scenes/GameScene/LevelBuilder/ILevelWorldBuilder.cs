using App.Scripts.Scenes.GameScene.Levels;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.LevelManager
{
    public interface ILevelWorldBuilder
    {
        UniTask Build(LevelData levelData);
    }
}