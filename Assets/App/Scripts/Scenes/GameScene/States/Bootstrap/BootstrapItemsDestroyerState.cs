using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapItemsDestroyerState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IItemsDestroyable _itemsDestroyable;
        private readonly BombDestroyService _bombDestroyService;
        private readonly BallSpeedBoostsDestroyer _ballSpeedBoostsDestroyer;
        private readonly DirectionBombDestroyService _directionBombDestroyService;
        private readonly ChainDestroyer _chainDestroyer;

        public BootstrapItemsDestroyerState(
            IStateMachine stateMachine,
            IItemsDestroyable itemsDestroyable,
            BombDestroyService bombDestroyService,
            BallSpeedBoostsDestroyer ballSpeedBoostsDestroyer,
            DirectionBombDestroyService directionBombDestroyService,
            ChainDestroyer chainDestroyer
        )
        {
            _stateMachine = stateMachine;
            _itemsDestroyable = itemsDestroyable;
            _bombDestroyService = bombDestroyService;
            _ballSpeedBoostsDestroyer = ballSpeedBoostsDestroyer;
            _directionBombDestroyService = directionBombDestroyService;
            _chainDestroyer = chainDestroyer;
        }
        
        public async UniTask Enter()
        {
            InitializeItemsDestroyable();

            _stateMachine.Enter<BootstrapLoadLevelState>();
            
            await UniTask.CompletedTask;
        }

        private void InitializeItemsDestroyable()
        {
            _itemsDestroyable.AsyncInitialize(new[]
            {
                BuildDestroyDataService(BoostTypeId.Bomb, _bombDestroyService),

                BuildDestroyDataService(BoostTypeId.BallAcceleration, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.BallSlowdown, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.PlayerShapeAddSize, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.PlayerShapeMinusSize, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.PlayerShapeAddSpeed, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.PlayerShapeMinusSpeed, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.AddHealth, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.MinusHealth, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.Fireball, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.ChainBomb, _chainDestroyer),
                BuildDestroyDataService(BoostTypeId.StickyPlatform, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.MiniGun, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.Autopilot, _ballSpeedBoostsDestroyer),
                BuildDestroyDataService(BoostTypeId.HorizontalBomb, _directionBombDestroyService),
                BuildDestroyDataService(BoostTypeId.VerticalBomb, _directionBombDestroyService),
            });
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
        
        private DestroyServiceData BuildDestroyDataService(BoostTypeId boostTypeId, IBlockDestroyService blockDestroyService)
        {
            return new()
            {
                BoostTypeId = boostTypeId,
                BlockDestroyService = blockDestroyService
            };
        }
    }
}