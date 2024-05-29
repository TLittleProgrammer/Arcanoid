﻿using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Bird;
using App.Scripts.Scenes.GameScene.Features.Levels.Animations;
using App.Scripts.Scenes.GameScene.Features.Walls;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapInitializeOtherServicesState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IWallLoader _wallLoader;
        private readonly IPopupProvider _popupProvider;
        private readonly IShowLevelAnimation _showLevelAnimation;
        private readonly IBallsService _ballsService;
        private readonly BallView.Factory _ballViewFactory;
        private readonly IBirdsService _birdsService;
        private readonly BirdView.Factory _birdViewFactory;

        public BootstrapInitializeOtherServicesState(
            IStateMachine stateMachine,
            IWallLoader wallLoader,
            IPopupProvider popupProvider,
            IShowLevelAnimation showLevelAnimation,
            IBallsService ballsService,
            BallView.Factory ballViewFactory,
            IBirdsService birdsService,
            BirdView.Factory birdViewFactory)
        {
            _stateMachine = stateMachine;
            _wallLoader = wallLoader;
            _popupProvider = popupProvider;
            _showLevelAnimation = showLevelAnimation;
            _ballsService = ballsService;
            _ballViewFactory = ballViewFactory;
            _birdsService = birdsService;
            _birdViewFactory = birdViewFactory;
        }
        
        public async UniTask Enter()
        {
            _ballsService.AddBall(_ballViewFactory.Create());

            BirdView birdView = _birdViewFactory.Create();
            _birdsService.AddBird(birdView);
            _birdsService.GoFly(birdView);

            await _wallLoader.AsyncInitialize();
            await _popupProvider.AsyncInitialize(Pathes.PathToPopups);
            await _showLevelAnimation.Show();

            _stateMachine.Enter<BootstrapInitializeAllRestartablesAndTickablesListsState>();
            
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}