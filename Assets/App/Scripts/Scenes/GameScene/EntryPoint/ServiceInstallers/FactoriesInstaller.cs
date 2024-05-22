using App.Scripts.External.Components;
using App.Scripts.General.Popup;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.General.Popup.Factory;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Effects;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Factories.CircleEffect;
using App.Scripts.Scenes.GameScene.Factories.Entity;
using App.Scripts.Scenes.GameScene.Factories.Health;
using App.Scripts.Scenes.GameScene.Factories.OnTopSprite;
using App.Scripts.Scenes.GameScene.Healthes.View;
using App.Scripts.Scenes.GameScene.TopSprites;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class FactoriesInstaller : Installer<RootUIViewProvider, FactoriesInstaller>
    {
        private readonly RootUIViewProvider _rootUIViewProvider;

        public FactoriesInstaller(RootUIViewProvider rootUIViewProvider)
        {
            _rootUIViewProvider = rootUIViewProvider;
        }
        
        public override void InstallBindings()
        {
            Container.BindFactory<string, IEntityView, IEntityView.Factory>().FromFactory<EntityFactory>();
            Container.BindFactory<ITransformable, IHealthPointView, IHealthPointView.Factory>().FromFactory<HealthFactory>();
            Container.BindFactory<EntityView, CircleEffect, CircleEffect.Factory>().FromFactory<CircleEffectFactory>();
            Container.BindFactory<IEntityView, OnTopSprites, OnTopSprites.Factory>().FromFactory<OnTopSpriteFactory>();
            
            Container.Bind<IPopupProvider>().To<ResourcesPopupProvider>().AsSingle();
            Container.Bind<IPopupFactory>().To<PopupFactory>().AsSingle();
            Container.Bind<IPopupService>().To<PopupService>().AsSingle().WithArguments(_rootUIViewProvider.BackPopupPlane);
        }
    }
}