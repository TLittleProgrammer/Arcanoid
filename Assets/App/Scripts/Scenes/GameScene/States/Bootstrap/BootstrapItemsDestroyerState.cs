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
        private readonly BoostBlockDestroyer _boostBlockDestroyer;
        private readonly DirectionBombDestroyService _directionBombDestroyService;
        private readonly CaptiveDestroyService _captiveDestroyService;
        private readonly ChainDestroyer _chainDestroyer;

        public BootstrapItemsDestroyerState(
            IStateMachine stateMachine,
            IItemsDestroyable itemsDestroyable,
            BombDestroyService bombDestroyService,
            BoostBlockDestroyer boostBlockDestroyer,
            DirectionBombDestroyService directionBombDestroyService,
            CaptiveDestroyService captiveDestroyService,
            ChainDestroyer chainDestroyer
        )
        {
            _stateMachine = stateMachine;
            _itemsDestroyable = itemsDestroyable;
            _bombDestroyService = bombDestroyService;
            _boostBlockDestroyer = boostBlockDestroyer;
            _directionBombDestroyService = directionBombDestroyService;
            _captiveDestroyService = captiveDestroyService;
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

                BuildDestroyDataService(BoostTypeId.BallAcceleration, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.BallSlowdown, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.PlayerShapeAddSize, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.PlayerShapeMinusSize, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.PlayerShapeAddSpeed, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.PlayerShapeMinusSpeed, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.AddHealth, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.MinusHealth, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.Fireball, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.ChainBomb, _chainDestroyer),
                BuildDestroyDataService(BoostTypeId.StickyPlatform, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.MiniGun, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.Autopilot, _boostBlockDestroyer),
                BuildDestroyDataService(BoostTypeId.HorizontalBomb, _directionBombDestroyService),
                BuildDestroyDataService(BoostTypeId.VerticalBomb, _directionBombDestroyService),
                BuildDestroyDataService(BoostTypeId.CaptiveBall, _captiveDestroyService),
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