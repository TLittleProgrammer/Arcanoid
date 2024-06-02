using System;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.General.Energy;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Levels;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.States;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours;

namespace App.Scripts.Scenes.MainMenuScene.Command
{
    public class LoadLevelCommand : ILoadLevelCommand
    {
        private readonly IEnergyDataService _energyDataService;
        private readonly IStateMachine _stateMachine;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly EnergyViewModel _energyViewModel;

        public LoadLevelCommand(
            IEnergyDataService energyDataService,
            IStateMachine stateMachine,
            ILevelPackInfoService levelPackInfoService,
            EnergyViewModel energyViewModel)
        {
            _energyDataService = energyDataService;
            _stateMachine = stateMachine;
            _levelPackInfoService = levelPackInfoService;
            _energyViewModel = energyViewModel;
        }
        
        public void Execute(LevelItemData itemData, int targetLevelIndex)
        {
            if (itemData.LevelPack.EnergyPrice > _energyDataService.CurrentValue)
            {
                return;
            }
            
            _energyDataService.Add(-itemData.LevelPack.EnergyPrice);
            
            _levelPackInfoService.LevelPackTransferData =
                new LevelPackTransferData
                {
                    NeedLoadLevel = true,
                    LevelIndex = targetLevelIndex,
                    LevelPack = itemData.LevelPack,
                    PackIndex = itemData.PackIndex
                };

            _energyViewModel.Dispose();

            _stateMachine.Enter<LoadingSceneState, string>(SceneNaming.Game);
        }
    }
}