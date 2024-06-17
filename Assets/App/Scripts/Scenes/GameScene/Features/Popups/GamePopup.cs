using System.Collections.Generic;
using App.Scripts.External.Popup;
using App.Scripts.General.Command;
using App.Scripts.General.Components;
using UnityEditor;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public abstract class GamePopup : PopupView
    {
        private IDisableButtonsCommand _disableButtonsCommand;

        public virtual void Initialize(IDisableButtonsCommand disableButtonsCommand)
        {
            _disableButtonsCommand = disableButtonsCommand;
        }

        protected void ExecuteCommand(ICommand command, List<Button> buttons)
        {
            _disableButtonsCommand.Execute(buttons);
            command.Execute();
        }
    }
}