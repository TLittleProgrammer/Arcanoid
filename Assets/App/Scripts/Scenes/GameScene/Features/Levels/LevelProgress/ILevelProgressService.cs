using System;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress
{
    public interface ILevelProgressService : IInitializable, IGeneralRestartable
    {
        event Action<float> ProgressChanged; 
        event Action LevelPassed;
        void CalculateStepByLevelData(LevelData levelData);
        void TakeOneStep();
        void SetDestroyableEntityCounter(int counter);
    }
}