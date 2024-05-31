using App.Scripts.External.Components;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.Popup;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.General.Popup.Factory;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.UI;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Factories;
using App.Scripts.Scenes.GameScene.Features.Factories.Boosts;
using App.Scripts.Scenes.GameScene.Features.Factories.CircleEffect;
using App.Scripts.Scenes.GameScene.Features.Factories.Entity;
using App.Scripts.Scenes.GameScene.Features.Factories.Health;
using App.Scripts.Scenes.GameScene.Features.Factories.OnTopSprite;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class FactoriesInstaller : Installer<RootUIViewProvider, BoostItemView, FactoriesInstaller>
    {
        private readonly RootUIViewProvider _rootUIViewProvider;
        private readonly BoostItemView _prefab;

        public FactoriesInstaller(RootUIViewProvider rootUIViewProvider, BoostItemView prefab)
        {
            _rootUIViewProvider = rootUIViewProvider;
            _prefab = prefab;
        }
        
        public override void InstallBindings()
        {
            Container.BindFactory<string, IEntityView, IEntityView.Factory>().FromFactory<EntityFactory>();
            Container.BindFactory<ITransformable, IHealthPointView, IHealthPointView.Factory>().FromFactory<HealthFactory>();
            Container.BindFactory<EntityView, CircleEffects, CircleEffects.Factory>().FromFactory<CircleEffectFactory>();
            Container.BindFactory<IEntityView, OnTopSprites, OnTopSprites.Factory>().FromFactory<OnTopSpriteFactory>();
            Container.BindFactory<BoostTypeId, BoostView, BoostView.Factory>().FromFactory<BoostViewFactory>();
            Container.BindFactory<BallView, BallView.Factory>().FromFactory<BallViewFactory>();
            Container.BindFactory<BirdView, BirdView.Factory>().FromFactory<BirdViewFactory>();

            Container.Bind<BoostItemView>().FromInstance(_prefab).WhenInjectedInto<BoostItemViewFactory>();
            Container.BindFactory<BoostTypeId, BoostItemView, BoostItemView.Factory>().FromFactory<BoostItemViewFactory>();
        }
    }
}