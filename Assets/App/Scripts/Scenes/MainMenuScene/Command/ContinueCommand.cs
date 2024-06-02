using App.Scripts.General.Popup;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenuScene.Command
{
    public sealed class ContinueCommand : IContinueCommand
    {
        private readonly IPopupService _popupService;

        public ContinueCommand(IPopupService popupService)
        {
            _popupService = popupService;
        }

        public async void Execute(Transform popupView)
        {
            await popupView.DOScale(Vector3.zero, 1f).ToUniTask();

            _popupService.CloseAll().Forget();
        }
    }
}