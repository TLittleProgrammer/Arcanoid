using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices
{
    public class BoostBlockDestroyer : IBlockDestroyService, IInitializeByLevelProgress
    {
        private readonly IAnimatedDestroyService _animatedDestroyService;
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly BoostView.Factory _boostViewFactory;
        private readonly IBoostMoveService _boostMoveService;
        private readonly IBoostPositionChecker _boostPositionChecker;

        public BoostBlockDestroyer(
            IAnimatedDestroyService animatedDestroyService,
            SimpleDestroyService simpleDestroyService,
            BoostView.Factory boostViewFactory,
            IBoostMoveService boostMoveService,
            IBoostPositionChecker boostPositionChecker)
        {
            _animatedDestroyService = animatedDestroyService;
            _simpleDestroyService = simpleDestroyService;
            _boostViewFactory = boostViewFactory;
            _boostMoveService = boostMoveService;
            _boostPositionChecker = boostPositionChecker;
        }

        public void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            AddBoostOnMap(entityView.BoostTypeId, entityView.Position, entityView.Scale);
            DestroyBoostBlock(gridItemData, entityView).Forget();
        }

        private async UniTask DestroyBoostBlock(GridItemData gridItemData, IEntityView entityView)
        {
            gridItemData.CurrentHealth = -1;
            await _animatedDestroyService.Animate(entityView);

            _simpleDestroyService.Destroy(gridItemData, entityView);
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            foreach (SaveBoostViewData data in levelDataProgress.ViewBoostDatas)
            {
                AddBoostOnMap(data.BoostTypeId, new Vector2(data.PositionX, data.PositionY), new(data.ScaleX, data.ScaleY));
            }
        }

        private void AddBoostOnMap(string boostTypeId, Vector3 entityViewPosition, Vector2 scale)
        {
            BoostView boostView = _boostViewFactory.Create(boostTypeId);
            boostView.Transform.position = entityViewPosition;

            boostView.Transform.DOScale(scale, 0.25f).From(Vector3.zero);

            _boostMoveService.AddView(boostView);
            _boostPositionChecker.Add(boostView);
        }
    }
}