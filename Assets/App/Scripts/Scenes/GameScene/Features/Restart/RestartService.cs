using App.Scripts.External.GameStateMachine;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Levels;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Features.States;

namespace App.Scripts.Scenes.GameScene.Features.Restart
{
    public sealed class RestartService : IRestartService
    {
        private readonly IEnergyDataService _energyDataService;
        private readonly IStateMachine _stateMachine;
        private readonly ILevelPackInfoService _levelPackInfoService;

        public RestartService(
            IEnergyDataService energyDataService,
            IStateMachine stateMachine,
            ILevelPackInfoService levelPackInfoService)
        {
            _energyDataService = energyDataService;
            _stateMachine = stateMachine;
            _levelPackInfoService = levelPackInfoService;
        }
        
        public void TryRestart()
        {
            LevelPack levelPack = _levelPackInfoService.GetDataForCurrentPack();

            if (levelPack.EnergyPrice <= _energyDataService.CurrentValue)
            {
                _energyDataService.Add(-levelPack.EnergyPrice);
                _stateMachine.Enter<RestartState>();
            }
        }
    }
}