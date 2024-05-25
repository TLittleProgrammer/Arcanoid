using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Levels.Load;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices
{
    public class DirectionBombDestroyService : IBlockDestroyService
    {
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IItemsDestroyable _itemsDestroyable;
        private readonly ILevelLoader _levelLoader;
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly IAnimatedDestroyService _animatedDestroyService;

        public DirectionBombDestroyService(
            ILevelViewUpdater levelViewUpdater,
            IItemsDestroyable itemsDestroyable,
            ILevelLoader levelLoader,
            SimpleDestroyService simpleDestroyService,
            IAnimatedDestroyService animatedDestroyService)
        {
            _levelViewUpdater = levelViewUpdater;
            _itemsDestroyable = itemsDestroyable;
            _levelLoader = levelLoader;
            _simpleDestroyService = simpleDestroyService;
            _animatedDestroyService = animatedDestroyService;
        }
        
        public void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            GetDirection(entityView, out Direction firstDirection, out Direction secondDirection);

            int2 initialPoint = new int2(entityView.GridPositionX, entityView.GridPositionY);
            int2[] firstDirectionPoints = GetAllPoints(initialPoint, firstDirection);
            int2[] secondDirectionPoints = GetAllPoints(initialPoint, secondDirection);

            List<EntityData> firstEntityDatas = GetEntityDatas(firstDirectionPoints);
            List<EntityData> secondEntityDatas = GetEntityDatas(secondDirectionPoints);

            DestroyAll(new(gridItemData, entityView), firstEntityDatas, secondEntityDatas);
        }

        private async void DestroyAll(EntityData currentBlock, List<EntityData> firstEntityDatas, List<EntityData> secondEntityDatas)
        {
            int maxSize = Math.Max(firstEntityDatas.Count, secondEntityDatas.Count);

            await _animatedDestroyService.Animate(new List<EntityData> { currentBlock });
            _simpleDestroyService.Destroy(currentBlock.GridItemData, currentBlock.EntityView);
            
            for (int i = 0; i < maxSize; i++)
            {
                if (i < firstEntityDatas.Count)
                {
                    _itemsDestroyable.Destroy(firstEntityDatas[i].GridItemData, firstEntityDatas[i].EntityView);
                }
                
                if (i < secondEntityDatas.Count)
                {
                    _itemsDestroyable.Destroy(secondEntityDatas[i].GridItemData, secondEntityDatas[i].EntityView);
                }

                await UniTask.Delay(350);
            }
        }

        private List<EntityData> GetEntityDatas(int2[] simpleCorrectGridPosition)
        {
            List<EntityData> result = new();

            foreach (int2 position in simpleCorrectGridPosition)
            {
                GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[new Vector2Int(position.x, position.y)];
                IEntityView entityView = _levelLoader.Entities.First(x => x.GridPositionX == position.x && x.GridPositionY == position.y);

                if (!entityView.BoxCollider2D.enabled)
                {
                    continue;
                }
                
                entityView.BoxCollider2D.enabled = false;
                result.Add(new()
                {
                    GridItemData = gridItemData,
                    EntityView = entityView
                });
            }

            return result;
        }



        private int2[] GetAllPoints(int2 initialPoint, Direction direction)
        {
            List<int2> result = new();

            initialPoint += direction.ToVector();
            
            while (initialPoint.x >= 0 &&
                   initialPoint.y >= 0 &&
                   initialPoint.x < _levelViewUpdater.LevelGridItemData.Width &&
                   initialPoint.y < _levelViewUpdater.LevelGridItemData.Height)
            {
                result.Add(initialPoint);
                initialPoint += direction.ToVector();
            }


            return result.ToArray();
        }

        private void GetDirection(IEntityView entityView, out Direction firstDirection, out Direction secondDirection)
        {
            if (entityView.BoostTypeId is BoostTypeId.HorizontalBomb)
            {
                firstDirection = Direction.Left;
                secondDirection = Direction.Right;
            }
            else
            {
                firstDirection = Direction.Up;
                secondDirection = Direction.Down;
            }
        }
    }
}