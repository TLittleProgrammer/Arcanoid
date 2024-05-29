using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.Settings;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.Boosts
{
    public class BoostViewFactory : IFactory<BoostTypeId, BoostView>
    {
        private readonly BoostViewProvider _boostViewProvider;
        private readonly IPoolContainer _poolContainer;

        public BoostViewFactory(BoostViewProvider boostViewProvider, IPoolContainer poolContainer)
        {
            _boostViewProvider = boostViewProvider;
            _poolContainer = poolContainer;
        }
        
        public BoostView Create(BoostTypeId targetType)
        {
            BoostView boostView = _poolContainer.GetItem<BoostView>(PoolTypeId.Boosts);

            boostView.SpriteRenderer.sprite = _boostViewProvider.Sprites[targetType];
            boostView.BoostTypeId = targetType;

            return boostView;
        }
    }
}