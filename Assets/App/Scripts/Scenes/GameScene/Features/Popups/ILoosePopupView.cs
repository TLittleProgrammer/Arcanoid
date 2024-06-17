using App.Scripts.External.Components;
using App.Scripts.General.Command;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Loose;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public interface ILoosePopupView : IGameObjectable
    {
        Button RestartButton { get; }

        void Initialize(LooseViewModel viewModel,
            EnergyViewModel energyViewModel,
            IEnergyDataService energyDataService, 
            IRestartCommand restartCommand,
            IBackCommand backCommand,
            IDisableButtonsCommand disableButtonsCommand,
            IBuyHealthCommand buyHealthCommand);
    }
}