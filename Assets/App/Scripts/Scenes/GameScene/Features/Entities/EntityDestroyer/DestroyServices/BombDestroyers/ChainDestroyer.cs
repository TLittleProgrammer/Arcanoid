using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.ObjectPool;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Entities.Extensions;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;
using App.Scripts.Scenes.GameScene.Features.Settings;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices.BombDestroyers
{
    public class ChainDestroyer : BombDestroyer
    {
        private readonly ILevelLoader _levelLoader;
        private readonly IAnimatedDestroyService _animatedDestroyService;
        private readonly SimpleDestroyService _simpleDestroyService;

        public ChainDestroyer(
            ILevelViewUpdater levelViewUpdater,
            ILevelLoader levelLoader,
            IAnimatedDestroyService animatedDestroyService,
            SimpleDestroyService simpleDestroyService,
            DestroyEntityEffectMapping destroyEntityEffectMapping,
            IKeyObjectPool<IEffect> keyObjectPool) : base(levelViewUpdater, destroyEntityEffectMapping, keyObjectPool)
        {
            _levelLoader = levelLoader;
            _animatedDestroyService = animatedDestroyService;
            _simpleDestroyService = simpleDestroyService;
        }

        public override void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            int2 initialPoint = new(entityView.GridPositionX, entityView.GridPositionY);

            Dictionary<int, List<EntityData>> allPoints = GetPoints(initialPoint);

            if (allPoints.Count == 0)
            {
                DestroyOnlyCurrentBlock(gridItemData, entityView).Forget();
            }
            else
            {
                DestroyChains(gridItemData, entityView, allPoints, initialPoint).Forget();
            }
        }

        private async UniTask DestroyChains(GridItemData gridItemData, IEntityView entityView, Dictionary<int, List<EntityData>> needDestroyPoints, int2 initialPoint)
        {
            await DestroyOnlyCurrentBlock(gridItemData, entityView);
            await DestroyOtherBlocks(needDestroyPoints, initialPoint);
        }

        private async UniTask DestroyOtherBlocks(Dictionary<int, List<EntityData>> needDestroyPoints, int2 initialPoint)
        {
            int id = GetEntityDataIdThatNeedDestroy(needDestroyPoints);

            List<int2> usablePoints = new();

            Queue<int2> pointsQueue = new Queue<int2>();
            Queue<int2> subPointsQueue = new Queue<int2>();
            List<int2> aroundInitialPoint = GetAroundPoints(initialPoint);

            pointsQueue.Enqueue(aroundInitialPoint[0]);
            pointsQueue.Enqueue(aroundInitialPoint[1]);
            pointsQueue.Enqueue(aroundInitialPoint[2]);
            pointsQueue.Enqueue(aroundInitialPoint[3]);

            usablePoints.Add(initialPoint);

            while (true)
            {
                if (await CheckMainQueue(pointsQueue, subPointsQueue)) break;

                int2 currentPoint = pointsQueue.Dequeue();

                if (!PointOnMap(currentPoint))
                    continue;

                IEntityView entity = _levelLoader.Entities.GetByCoordinates(currentPoint);
                EntityStage entityStage = LevelViewUpdater.GetEntityStage(entity);

                if (ThisBlockIsNotSimple(entityStage, entity) || usablePoints.Contains(currentPoint) || entity.EntityId != id)
                {
                    continue;
                }

                usablePoints.Add(currentPoint);

                SimpleDestroy(LevelViewUpdater.LevelGridItemData[entity.GridPositionX, entity.GridPositionY], entity).Forget();
                AddPointsToQueue(currentPoint, usablePoints, subPointsQueue);
            }
        }

        private async UniTask<bool> CheckMainQueue(Queue<int2> pointsQueue, Queue<int2> subPointsQueue)
        {
            if (pointsQueue.Count == 0)
            {
                if (subPointsQueue.Count == 0)
                {
                    return true;
                }

                while (subPointsQueue.Count != 0)
                {
                    pointsQueue.Enqueue(subPointsQueue.Dequeue());
                }

                await UniTask.Delay(150);
            }

            return false;
        }

        private void AddPointsToQueue(int2 currentPoint, List<int2> usablePoints, Queue<int2> subPointsQueue)
        {
            List<int2> aroundPoints = GetAroundPoints(currentPoint);

            foreach (int2 point in aroundPoints)
            {
                if (!usablePoints.Contains(point))
                {
                    subPointsQueue.Enqueue(point);
                }
            }
        }

        private int GetEntityDataIdThatNeedDestroy(Dictionary<int,List<EntityData>> needDestroyPoints)
        {
            int max = -1;
            List<EntityData> result = null;

            foreach (KeyValuePair<int,List<EntityData>> point in needDestroyPoints)
            {
                if (point.Value.Count > max)
                {
                    result = point.Value;
                    max = point.Value.Count;
                }
            }

            return result[0].EntityView.EntityId;
        }

        private Dictionary<int, List<EntityData>> GetPoints(int2 initialPoint)
        {
            List<int2> usablePoints = new();
            Dictionary<int, List<EntityData>> counter = new();

            Queue<KeyValuePair<int2, int>> points = new Queue<KeyValuePair<int2, int>>();
            List<int2> aroundInitialPoint = GetAroundPoints(initialPoint);
            
            points.Enqueue(new(aroundInitialPoint[0], -1));
            points.Enqueue(new(aroundInitialPoint[1], -1));
            points.Enqueue(new(aroundInitialPoint[2], -1));
            points.Enqueue(new(aroundInitialPoint[3], -1));
            
            usablePoints.Add(initialPoint);

            while (points.Count != 0)
            {
                KeyValuePair<int2, int> currentPoint = points.Dequeue();
                
                if(!PointOnMap(currentPoint.Key))
                    continue;

                IEntityView entity = _levelLoader.Entities.First(x => x.GridPositionX == currentPoint.Key.x && x.GridPositionY == currentPoint.Key.y);
                EntityStage entityStage = LevelViewUpdater.GetEntityStage(entity);

                if (ThisBlockIsNotSimple(entityStage, entity) ||
                    PreviousEntityNotEqualsCurrent(currentPoint, entity) ||
                    usablePoints.Contains(currentPoint.Key))
                {
                    continue;
                }
                
                usablePoints.Add(currentPoint.Key);

                EntityData entityData = CreateEntityData(entity);

                UpdateCounter(counter, entity, entityData);
                AddAroundCurrentPoints(currentPoint.Key, usablePoints, points, entityData);
            }
            
            return counter;
        }

        private EntityData CreateEntityData(IEntityView entity)
        {
            EntityData entityData = new EntityData();
            entityData.GridItemData = LevelViewUpdater.LevelGridItemData[entity.GridPositionX, entity.GridPositionY];
            entityData.EntityView = entity;

            return entityData;
        }

        private bool PreviousEntityNotEqualsCurrent(KeyValuePair<int2, int> currentPoint, IEntityView entity)
        {
            if (currentPoint.Value != -1)
            {
                if (currentPoint.Value != entity.EntityId)
                    return true;
            }

            return false;
        }

        private void AddAroundCurrentPoints(int2 currentPoint, List<int2> usablePoints, Queue<KeyValuePair<int2, int>> points, EntityData entityData)
        {
            List<int2> aroundPoints = GetAroundPoints(currentPoint);

            foreach (int2 point in aroundPoints)
            {
                if (!usablePoints.Contains(point))
                {
                    points.Enqueue(new(point, entityData.EntityView.EntityId));
                }
            }
        }

        private void UpdateCounter(Dictionary<int, List<EntityData>> counter, IEntityView entity, EntityData entityData)
        {
            if (!counter.ContainsKey(entity.EntityId))
            {
                counter.Add(entity.EntityId, new()
                {
                    entityData
                });
            }
            else
            {
                counter[entity.EntityId].Add(entityData);
            }
        }

        private bool ThisBlockIsNotSimple(EntityStage entityStage, IEntityView entity)
        {
            return entityStage.ICanGetDamage is false || entity.BoxCollider2D.enabled == false || (entityStage.BoostTypeId != String.Empty && entityStage.BoostTypeId != "0");
        }

        private async UniTask DestroyOnlyCurrentBlock(GridItemData gridItemData, IEntityView entityView)
        {
            await SimpleDestroy(gridItemData, entityView);
        }

        private async UniTask SimpleDestroy(GridItemData gridItemData, IEntityView entityView)
        {
            SetExplosionsEffect(entityView);
            gridItemData.CurrentHealth = -1;
            
            await _animatedDestroyService.Animate(entityView);
            _simpleDestroyService.Destroy(gridItemData, entityView);
        }
    }
}