using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Ball;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.LevelProgress;
using App.Scripts.Scenes.GameScene.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Levels.Data;
using App.Scripts.Scenes.GameScene.Levels.Load;
using App.Scripts.Scenes.GameScene.Levels.View;
using App.Scripts.Scenes.GameScene.Pools;
using App.Scripts.Scenes.GameScene.TopSprites;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer.DestroyServices
{
    public sealed class BombDestroyService : IBlockDestroyService
    {
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly ILevelLoader _levelLoader;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IPoolContainer _poolContainer;
        private readonly IBallSpeedUpdater _ballSpeedUpdater;
        private readonly IItemViewDamageService _itemViewDamageService;

        public BombDestroyService(
            ILevelViewUpdater levelViewUpdater,
            ILevelLoader levelLoader,
            ILevelProgressService levelProgressService,
            IPoolContainer poolContainer,
            IBallSpeedUpdater ballSpeedUpdater,
            IItemViewDamageService itemViewDamageService)
        {
            _levelViewUpdater = levelViewUpdater;
            _levelLoader = levelLoader;
            _levelProgressService = levelProgressService;
            _poolContainer = poolContainer;
            _ballSpeedUpdater = ballSpeedUpdater;
            _itemViewDamageService = itemViewDamageService;
        }
        
        public async void Destroy(GridItemData gridItemData, IEntityView iEntityView)
        {
            EntityView entityView = iEntityView as EntityView;

            int2 initialPosition = new int2(entityView.GridPositionX, entityView.GridPositionY);
            
            int2[] simpleCorrectGridPosition = FilterGridPoints(initialPosition, 
                new []
                {
                    Direction.Left,
                    Direction.Right,
                    Direction.Up,
                    Direction.Down
                });
            
            int2[] diagonalsCorrectGridPosition = FilterGridPoints(initialPosition, 
                new []
                {
                    Direction.UpLeft,
                    Direction.UpRight,
                    Direction.DownLeft,
                    Direction.DownRight
                });

            List<EntityData> immediateDatas = GetEntityDatas(simpleCorrectGridPosition, gridItemData.Damage);
            List<EntityData> diagonalsDatas = GetEntityDatas(diagonalsCorrectGridPosition, gridItemData.Damage);

            await AnimateAll(entityView, immediateDatas, diagonalsDatas);

            DestroyAllEntites(new()
            {
                EntityView = entityView,
                GridItemData = gridItemData
            },
                immediateDatas, diagonalsDatas);
        }

        private async UniTask AnimateAll(EntityView bomb, List<EntityData> immediateEntityDatas, List<EntityData> diagonalsEntityDatas)
        {
            await AnimateDataList(new List<EntityData>()
            {
                new()
                {
                    GridItemData = null,
                    EntityView = bomb
                }
            });
            await AnimateDataList(immediateEntityDatas);
            await AnimateDataList(diagonalsEntityDatas);
        }

        private async UniTask AnimateDataList(List<EntityData> immediateEntityDatas)
        {
            foreach (EntityData data in immediateEntityDatas)
            {
                Transform transform = data.EntityView.GameObject.transform;

                transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InBack).ToUniTask().Forget();
            }
            
            await UniTask.Delay(350);
        }

        private void DestroyAllEntites(EntityData bomb, List<EntityData> immediateDatas, List<EntityData> diagonalDatas)
        {
            DestroyEntities(new List<EntityData>
            {
                bomb
            });
            DestroyEntities(immediateDatas);
            DestroyEntities(diagonalDatas);
        }

        private void DestroyEntities(List<EntityData> immediateDatas)
        {
            foreach (EntityData entityData in immediateDatas)
            {
                _levelProgressService.TakeOneStep();

                _poolContainer.RemoveItem(PoolTypeId.EntityView, entityData.EntityView as EntityView);
                foreach (OnTopSprites sprite in entityData.GridItemData.Sprites)
                {
                    _poolContainer.RemoveItem(PoolTypeId.OnTopSprite, sprite);
                }

                _ballSpeedUpdater.UpdateSpeed();
            }
        }

        private List<EntityData> GetEntityDatas(int2[] simpleCorrectGridPosition, int damage)
        {
            List<EntityData> result = new();
            
            foreach (int2 position in simpleCorrectGridPosition)
            {
                GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[new Vector2Int(position.x, position.y)];
                IEntityView entityView = _levelLoader.Entities.First(x => x.GridPositionX == position.x && x.GridPositionY == position.y);

                
                if (gridItemData.CurrentHealth - damage <= 0)
                {
                    entityView.BoxCollider2D.enabled = false;
                    result.Add(new()
                    {
                        GridItemData = gridItemData,
                        EntityView = entityView
                    });
                    continue;
                }

                int currentHealth = gridItemData.CurrentHealth;

                gridItemData.CurrentHealth -= damage;
                EntityStage entityStage = _levelViewUpdater.GetEntityStage(entityView);

                for (int i = currentHealth; i >= gridItemData.CurrentHealth; i--)
                {
                    _itemViewDamageService.TryAddOnTopSprite(entityView, entityStage, gridItemData, i);
                }
            }

            return result;
        }

        private int2[] FilterGridPoints(int2 initialPoint, Direction[] directions)
        {
            List<int2> result = new();

            foreach (Direction direction in directions)
            {
                int2 targetPoint = initialPoint + direction.ToVector();

                if (targetPoint.x >= 0 &&
                    targetPoint.y >= 0 &&
                    targetPoint.x < _levelViewUpdater.LevelGridItemData.Width &&
                    targetPoint.y < _levelViewUpdater.LevelGridItemData.Height)
                {
                    result.Add(targetPoint);
                }
            }
            
            return result.ToArray();
        }
    }
}