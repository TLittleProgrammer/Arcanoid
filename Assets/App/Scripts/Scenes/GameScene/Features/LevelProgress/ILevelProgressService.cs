using System;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Levels;
using Zenject;

namespace App.Scripts.Scenes.GameScene.LevelProgress
{
    public interface ILevelProgressService : IInitializable, IRestartable
    {
        event Action LevelPassed;
        void CalculateStepByLevelData(LevelData levelData);
        void TakeOneStep();
    }
}