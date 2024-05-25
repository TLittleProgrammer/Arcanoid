using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Levels.Load;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.TopSprites;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices
{
    public sealed class BombDestroyService : IBlockDestroyService
    {
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly ILevelLoader _levelLoader;
        private readonly IAnimatedDestroyService _animatedDestroyService;
        private readonly IAddVisualDamage _addVisualDamage;
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly IItemsDestroyable _itemsDestroyable;

        public BombDestroyService(
            ILevelViewUpdater levelViewUpdater,
            ILevelLoader levelLoader,
            IAnimatedDestroyService animatedDestroyService,
            IAddVisualDamage addVisualDamage,
            SimpleDestroyService simpleDestroyService,
            IItemsDestroyable itemsDestroyable)
        {
            _levelViewUpdater = levelViewUpdater;
            _levelLoader = levelLoader;
            _animatedDestroyService = animatedDestroyService;
            _addVisualDamage = addVisualDamage;
            _simpleDestroyService = simpleDestroyService;
            _itemsDestroyable = itemsDestroyable;
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
            await _animatedDestroyService.Animate(new List<EntityData>()
            {
                new()
                {
                    GridItemData = null,
                    EntityView = bomb
                }
            });

            foreach (EntityData data in immediateEntityDatas)
            {
                _itemsDestroyable.Destroy(data.GridItemData, data.EntityView);
            }

            await UniTask.Delay(350);

            
            foreach (EntityData data in diagonalsEntityDatas)
            {
                _itemsDestroyable.Destroy(data.GridItemData, data.EntityView);
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
                _simpleDestroyService.Destroy(entityData.GridItemData, entityData.EntityView);
            }
        }

        private List<EntityData> GetEntityDatas(int2[] simpleCorrectGridPosition, int damage)
        {
            List<EntityData> result = new();
            
            foreach (int2 position in simpleCorrectGridPosition)
            {
                GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[new Vector2Int(position.x, position.y)];
                IEntityView entityView = _levelLoader.Entities.First(x => x.GridPositionX == position.x && x.GridPositionY == position.y);
                
                if (gridItemData.CurrentHealth - damage <= 0 && entityView.BoxCollider2D.enabled)
                {
                    entityView.BoxCollider2D.enabled = false;
                    result.Add(new()
                    {
                        GridItemData = gridItemData,
                        EntityView = entityView
                    });
                    continue;
                }

                _addVisualDamage.AddVisualDamage(damage, gridItemData, entityView);
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