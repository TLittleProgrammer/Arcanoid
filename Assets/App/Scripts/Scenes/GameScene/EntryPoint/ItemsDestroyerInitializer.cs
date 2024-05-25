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

        public ItemsDestroyerInitializer(
            IItemsDestroyable itemsDestroyable,
            BombDestroyService bombDestroyService,
            BallSpeedBoostsDestroyer ballSpeedBoostsDestroyer
            )
        {
            _itemsDestroyable = itemsDestroyable;
            _bombDestroyService = bombDestroyService;
            _ballSpeedBoostsDestroyer = ballSpeedBoostsDestroyer;
        }
        
        public void Initialize()
        {
            _itemsDestroyable.AsyncInitialize(new []
            {
                new DestroyServiceData
                {
                    BoostTypeId = BoostTypeId.Bomb,
                    BlockDestroyService = _bombDestroyService
                },
                new DestroyServiceData()
                {
                    BoostTypeId = BoostTypeId.BallAcceleration,
                    BlockDestroyService = _ballSpeedBoostsDestroyer
                },
                new DestroyServiceData()
                {
                    BoostTypeId = BoostTypeId.BallSlowdown,
                    BlockDestroyService = _ballSpeedBoostsDestroyer
                },
                new DestroyServiceData()
                {
                    BoostTypeId = BoostTypeId.PlayerShapeAddSize,
                    BlockDestroyService = _ballSpeedBoostsDestroyer
                },
                new DestroyServiceData()
                {
                    BoostTypeId = BoostTypeId.PlayerShapeMinusSize,
                    BlockDestroyService = _ballSpeedBoostsDestroyer
                }
                ,
                new DestroyServiceData()
                {
                    BoostTypeId = BoostTypeId.PlayerShapeAddSpeed,
                    BlockDestroyService = _ballSpeedBoostsDestroyer
                }
                ,
                new DestroyServiceData()
                {
                    BoostTypeId = BoostTypeId.PlayerShapeMinusSpeed,
                    BlockDestroyService = _ballSpeedBoostsDestroyer
                }
            });
        }
    }
}