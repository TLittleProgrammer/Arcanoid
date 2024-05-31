using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Command;
using Zenject;

namespace App.Scripts.Scenes.GameScene.MVVM.Main
{
    public sealed class MainViewModel : IInitializable
    {
        private readonly IClickable _openMenuPopupButton;
        private readonly IOpenMenuPopupCommand _openMenuPopupCommand;

        public MainViewModel(IClickable openMenuPopupButton, IOpenMenuPopupCommand openMenuPopupCommand)
        {
            _openMenuPopupButton = openMenuPopupButton;
            _openMenuPopupCommand = openMenuPopupCommand;
        }

        public void Initialize()
        {
            _openMenuPopupButton.Clicked += _openMenuPopupCommand.Execute;
        }
    }
}