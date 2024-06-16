using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.General.Popup;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.States.Gameloop;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Command.Loose
{
    public class BuyHealthCommand : IBuyHealthCommand
    {
        private readonly IPopupService _popupService;
        private readonly IEnergyDataService _energyDataService;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly IStateMachine _stateMachine;

        public BuyHealthCommand(
            IPopupService popupService,
            IEnergyDataService energyDataService,
            ILevelPackInfoService levelPackInfoService,
            IStateMachine stateMachine)
        {
            _popupService = popupService;
            _energyDataService = energyDataService;
            _levelPackInfoService = levelPackInfoService;
            _stateMachine = stateMachine;
        }
        
        public async void Execute()
        {
            _energyDataService.Add(-_levelPackInfoService.LevelPackTransferData.LevelPack.EnergyPrice * 2);
            await _popupService.CloseAll();

            _stateMachine.Enter<GameLoopState>().Forget();
        }
    }
}