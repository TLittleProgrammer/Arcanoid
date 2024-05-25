using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint
{
    public class ItemsDestroyerInitializer : IInitializable
    {
        private readonly IItemsDestroyable _itemsDestroyable;
        private readonly BombDestroyService _bombDestroyService;
        private readonly BallSpeedBoostsDestroyer _ballSpeedBoostsDestroyer;
        private readonly DirectionBombDestroyService _directionBombDestroyService;

        public ItemsDestroyerInitializer(
            IItemsDestroyable itemsDestroyable,
            BombDestroyService bombDestroyService,
            BallSpeedBoostsDestroyer ballSpeedBoostsDestroyer,
            DirectionBombDestroyService directionBombDestroyService
            )
        {
            _itemsDestroyable = itemsDestroyable;
            _bombDestroyService = bombDestroyService;
            _ballSpeedBoostsDestroyer = ballSpeedBoostsDestroyer;
            _directionBombDestroyService = directionBombDestroyService;
        }
        
        public void Initialize()
        {
            _itemsDestroyable.AsyncInitialize(new []
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
                BuildDestroyDataService(BoostTypeId.HorizontalBomb, _directionBombDestroyService),
                BuildDestroyDataService(BoostTypeId.VerticalBomb, _directionBombDestroyService),
            });
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