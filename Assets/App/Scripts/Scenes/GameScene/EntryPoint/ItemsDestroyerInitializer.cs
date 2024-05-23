using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer;
using App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer.DestroyServices;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint
{
    public class ItemsDestroyerInitializer : IInitializable
    {
        private readonly IItemsDestroyable _itemsDestroyable;
        private readonly BombDestroyService _bombDestroyService;

        public ItemsDestroyerInitializer(
            IItemsDestroyable itemsDestroyable,
            BombDestroyService bombDestroyService
            )
        {
            _itemsDestroyable = itemsDestroyable;
            _bombDestroyService = bombDestroyService;
        }
        
        public void Initialize()
        {
            _itemsDestroyable.AsyncInitialize(new []
            {
                new DestroyServiceData
                {
                    BoostTypeId = BoostTypeId.Bomb,
                    BlockDestroyService = _bombDestroyService
                }
            });
        }
    }
}