using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Tests.Healthes
{
    public class TestViewHealthPointService : IViewHealthPointService
    {
        public UniTask AsyncInitialize(LevelData param)
        {
            return UniTask.CompletedTask;
        }

        public void Restart()
        {
        }

        public void UpdateHealth(int healthCount)
        {
        }
    }
}