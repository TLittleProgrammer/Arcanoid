using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Levels;
using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.States;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Restart
{
    public sealed class RestartService : IRestartService, IInitializable
    {
        private readonly IEnergyDataService _energyDataService;
        private readonly IStateMachine _stateMachine;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly IHealthContainer _healthContainer;

        public RestartService(
            IEnergyDataService energyDataService,
            IStateMachine stateMachine,
            ILevelPackInfoService levelPackInfoService,
            IPlayerShapeMover playerShapeMover,
            IHealthContainer healthContainer)
        {
            _energyDataService = energyDataService;
            _stateMachine = stateMachine;
            _levelPackInfoService = levelPackInfoService;
            _playerShapeMover = playerShapeMover;
            _healthContainer = healthContainer;
        }

        public void Initialize()
        {
            _healthContainer.GetDamage += _ =>
            {
                RestartSession();
            };
        }

        public void TryRestartLevel()
        {
            LevelPack levelPack = _levelPackInfoService.GetDataForCurrentPack();

            if (levelPack is null)
            {
                _stateMachine.Enter<RestartState>();
                return;
            }
            
            if (levelPack.EnergyPrice <= _energyDataService.CurrentValue)
            {
                _energyDataService.Add(-levelPack.EnergyPrice);
                _stateMachine.Enter<RestartState>();
            }
        }

        public void RestartSession()
        {
            _playerShapeMover.Restart();
        }
    }
}