using App.Scripts.External.Components;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States
{
    public class MenuPopupState : IState<ITransformable>
    {
        private readonly IPopupService _popupService;

        private float _lastSpeedMultiplier;
        
        public MenuPopupState(IPopupService popupService)
        {
            _popupService = popupService;
        }
        
        public async UniTask Enter(ITransformable transformable)
        {
            _popupService.Show<MenuPopupView>(transformable);
            
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}