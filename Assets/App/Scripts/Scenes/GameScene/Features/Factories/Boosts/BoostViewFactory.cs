using App.Scripts.General.Providers;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.Settings;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.Boosts
{
    public class BoostViewFactory : IFactory<BoostTypeId, BoostView>
    {
        private readonly SpriteProvider _spriteProvider;
        private readonly BoostView.Pool _boostViewPool;

        public BoostViewFactory(SpriteProvider spriteProvider, BoostView.Pool boostViewPool)
        {
            _spriteProvider = spriteProvider;
            _boostViewPool = boostViewPool;
        }
        
        public BoostView Create(BoostTypeId boostId)
        {
            BoostView boostView = _boostViewPool.Spawn();

            boostView.SpriteRenderer.sprite = _spriteProvider.Sprites[boostId.ToString() + "_block"];
            boostView.BoostTypeId = boostId;

            return boostView;
        }
    }
}