﻿using App.Scripts.External.Components;
using App.Scripts.General.Popup;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.General.Popup.Factory;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.UI;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Factories.Boosts;
using App.Scripts.Scenes.GameScene.Features.Factories.CircleEffect;
using App.Scripts.Scenes.GameScene.Features.Factories.Entity;
using App.Scripts.Scenes.GameScene.Features.Factories.Health;
using App.Scripts.Scenes.GameScene.Features.Factories.OnTopSprite;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using App.Scripts.Scenes.GameScene.Features.TopSprites;
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
            Container.BindFactory<EntityView, CircleEffect, CircleEffect.Factory>().FromFactory<CircleEffectFactory>();
            Container.BindFactory<IEntityView, OnTopSprites, OnTopSprites.Factory>().FromFactory<OnTopSpriteFactory>();
            Container.BindFactory<BoostTypeId, BoostView, BoostView.Factory>().FromFactory<BoostViewFactory>();

            Container.Bind<BoostItemView>().FromInstance(_prefab).WhenInjectedInto<BoostItemViewFactory>();
            Container.BindFactory<BoostTypeId, BoostItemView, BoostItemView.Factory>().FromFactory<BoostItemViewFactory>();
            
            Container.Bind<IPopupProvider>().To<ResourcesPopupProvider>().AsSingle();
            Container.Bind<IPopupFactory>().To<PopupFactory>().AsSingle();
            Container.Bind<IPopupService>().To<PopupService>().AsSingle().WithArguments(_rootUIViewProvider.BackPopupPlane);
        }
    }
}