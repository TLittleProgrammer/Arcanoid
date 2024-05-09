using App.Scripts.Scenes.GameScene.Levels;
using App.Scripts.Scenes.GameScene.ScreenInfo;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.LevelManager
{
    public class LevelWorldBuilder : ILevelWorldBuilder
    {
        public LevelWorldBuilder()
        {
        }
        
        public async UniTask Build(LevelData levelData)
        {
            await UniTask.CompletedTask;
        }
    }
}