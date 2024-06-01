﻿using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Command;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Popups;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.MVVM.Popups.Menu
{
    public sealed class MenuViewModel
    {
        private readonly IContinueCommand _continueCommand;
        private readonly IBackCommand _backCommand;
        private readonly IRestartCommand _restartCommand;
        private readonly IDisableButtonsCommand _disableButtonsCommand;
        private readonly ITweenersLocator _tweenersLocator;

        private IMenuPopupView _menuPopupView;
        
        public MenuViewModel(
            IContinueCommand continueCommand,
            IBackCommand backCommand,
            IRestartCommand restartCommand,
            IDisableButtonsCommand disableButtonsCommand,
            ITweenersLocator tweenersLocator)
        {
            _continueCommand = continueCommand;
            _backCommand = backCommand;
            _restartCommand = restartCommand;
            _disableButtonsCommand = disableButtonsCommand;
            _tweenersLocator = tweenersLocator;
        }

        public void InitializeView(IMenuPopupView menuPopupView)
        {
            _menuPopupView = menuPopupView;
            SubscribeOnCommands(menuPopupView);
        }

        private void SubscribeOnCommands(IMenuPopupView menuPopupView)
        {
            menuPopupView.ContinueButton.onClick.AddListener(Continue);
            menuPopupView.RestartButton.onClick.AddListener(Restart);
            menuPopupView.BackButton.onClick.AddListener(Back);
        }

        private void Restart()
        {
            DisableButtons();
            _restartCommand.Execute();
        }

        private async void Continue()
        {
            DisableButtons();

            await CloseView();
            
            _continueCommand.Execute();
        }

        private void Back()
        {
            DisableButtons();

            _backCommand.Execute();
        }

        private UniTask CloseView()
        {
            Sequence sequence = DOTween.Sequence();

            sequence
                .Append(AnimateScale(_menuPopupView.ContinueButton.transform, Vector3.zero, 0.25f, Ease.OutBounce))
                .Append(AnimateScale(_menuPopupView.BackButton.transform, Vector3.zero, 0.25f, Ease.OutBounce))
                .Append(AnimateScale(_menuPopupView.RestartButton.transform, Vector3.zero, 0.25f, Ease.OutBounce))
                .Append(AnimateScale(_menuPopupView.Transform, Vector3.zero, 0.75f, Ease.InOutBounce));
            
            _tweenersLocator.AddSequence(sequence);

            return sequence.ToUniTask();
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> AnimateScale(Transform transform, Vector3 targetScale, float duration, Ease ease)
        {
            return transform.DOScale(targetScale, duration).SetEase(ease);
        }

        private void DisableButtons()
        {
            List<Button> buttons = new List<Button>()
            {
                _menuPopupView.BackButton,
                _menuPopupView.ContinueButton,
                _menuPopupView.RestartButton,
            };
            
            _disableButtonsCommand.Execute(buttons);
        }
    }
}