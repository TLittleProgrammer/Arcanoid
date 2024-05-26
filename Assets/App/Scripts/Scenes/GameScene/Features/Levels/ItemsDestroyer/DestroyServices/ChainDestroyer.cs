using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Levels.Load;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices
{
    public class ChainDestroyer : DestroyService
    {
        private readonly ILevelLoader _levelLoader;
        private readonly IAnimatedDestroyService _animatedDestroyService;
        private readonly SimpleDestroyService _simpleDestroyService;

        public ChainDestroyer(
            ILevelViewUpdater levelViewUpdater,
            ILevelLoader levelLoader,
            IAnimatedDestroyService animatedDestroyService,
            SimpleDestroyService simpleDestroyService) : base(levelViewUpdater)
        {
            _levelLoader = levelLoader;
            _animatedDestroyService = animatedDestroyService;
            _simpleDestroyService = simpleDestroyService;
        }
        
        public override void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            int2 initialPoint = new(entityView.GridPositionX, entityView.GridPositionY);
            
            int2[] left  = GetAllPointsByDirection(initialPoint, Direction.Left);
            int2[] right = GetAllPointsByDirection(initialPoint, Direction.Right);
            int2[] down  = GetAllPointsByDirection(initialPoint, Direction.Down);
            int2[] up    = GetAllPointsByDirection(initialPoint, Direction.Up);

            ChainData chainData = GetChainData(left, right, down, up);

            if (chainData.EntityType == Int32.MinValue)
            {
                DestroyOnlyCurrentBlock(gridItemData, entityView);
            }
            else
            {
                DestroyCurrentBlockAndChains(gridItemData, entityView, chainData, left, right, up, down);
            }
        }

        private async void DestroyCurrentBlockAndChains(GridItemData gridItemData, IEntityView entityView, ChainData chainData, int2[] left, int2[] right, int2[] up, int2[] down)
        {
            await SimpleDestroy(gridItemData, entityView);

            int maxLength = Math.Max(left.Length, Math.Max(right.Length, Math.Max(down.Length, up.Length)));

            List<int2[]> checkedChains = new List<int2[]>()
            {
                left, right, down, up
            };

            for (int i = 0; i < maxLength; i++)
            {
                List<IEntityView> needDestroyEntities = new();
                
                for (int j = 0; j < checkedChains.Count; j++)
                {
                    int2[] currentChain = checkedChains[j];
                    if (i >= currentChain.Length)
                    {
                        checkedChains.Remove(currentChain);
                        j--;
                        continue;
                    }
                    
                    IEntityView entity = _levelLoader.Entities.First(x => x.GridPositionX == currentChain[i].x && x.GridPositionY == currentChain[i].y);

                    if (chainData.EntityType != entity.EntityId)
                    {
                        checkedChains.Remove(currentChain);
                        j--;
                        continue;
                    }
                    
                    needDestroyEntities.Add(entity);
                }

                foreach (IEntityView entity in needDestroyEntities)
                {
                    GridItemData destroyableGridData = LevelViewUpdater.LevelGridItemData[entity.GridPositionX, entity.GridPositionY];

                    SimpleDestroy(destroyableGridData, entity).Forget();
                }

                await UniTask.Delay(350);
            }
        }

        private async void DestroyOnlyCurrentBlock(GridItemData gridItemData, IEntityView entityView)
        {
            await SimpleDestroy(gridItemData, entityView);
        }

        private async UniTask SimpleDestroy(GridItemData gridItemData, IEntityView entityView)
        {
            await _animatedDestroyService.Animate(entityView);

            gridItemData.CurrentHealth = -1;
            _simpleDestroyService.Destroy(gridItemData, entityView);
        }

        private ChainData GetChainData(int2[] left, int2[] right, int2[] down, int2[] up)
        {
            Dictionary<int, int> chainCounter = new();

            GoThroughPoints(ref chainCounter, left);
            GoThroughPoints(ref chainCounter, right);
            GoThroughPoints(ref chainCounter, down);
            GoThroughPoints(ref chainCounter, up);

            int max = Int32.MinValue;
            ChainData chainData = new();

            foreach (KeyValuePair<int,int> pair in chainCounter)
            {
                if (pair.Value >= max)
                {
                    max = pair.Value;
                    chainData.EntityType = pair.Key;
                }
            }

            return chainData;
        }

        private void GoThroughPoints(ref Dictionary<int,int> chainCounter, int2[] chain)
        {
            if (chain.Length == 0)
                return;

            int2 startPoint = chain[0];
            int counter = 1;
            IEntityView entity = _levelLoader.Entities.First(x => x.GridPositionX == startPoint.x && x.GridPositionY == startPoint.y);
            EntityStage entityStage = LevelViewUpdater.GetEntityStage(entity);

            if (entityStage.ICanGetDamage is false || entityStage.BoostTypeId != BoostTypeId.None || entity.BoxCollider2D.enabled == false)
                return;

            for (int i = 1; i < chain.Length; i++)
            {
                int2 nextPoint = chain[i];
                IEntityView nextEntity = _levelLoader.Entities.First(x => x.GridPositionX == nextPoint.x && x.GridPositionY == nextPoint.y);

                if (nextEntity.BoxCollider2D.enabled == false)
                {
                    break;
                }
                
                if (nextEntity.EntityId == entity.EntityId)
                {
                    counter++;
                }
            }

            if (chainCounter.ContainsKey(entity.EntityId))
            {
                chainCounter[entity.EntityId] += counter;
            }
            else
            {
                chainCounter.Add(entity.EntityId, counter);
            }
        }

        private sealed class ChainData
        {
            public int EntityType = Int32.MinValue;
        }
    }
}