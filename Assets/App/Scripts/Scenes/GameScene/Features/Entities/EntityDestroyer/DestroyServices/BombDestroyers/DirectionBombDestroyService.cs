using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using App.Scripts.External.ObjectPool;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Effects.Bombs;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using App.Scripts.Scenes.GameScene.Features.Settings;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices.BombDestroyers
{
    public class DirectionBombDestroyService : BombDestroyer
    {
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IEntityDestroyable _entityDestroyable;
        private readonly ILevelLoader _levelLoader;
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly IAnimatedDestroyService _animatedDestroyService;
        private readonly IScreenInfoProvider _screenInfoProvider;
        private readonly LaserEffect.Pool _laserEffectPool;

        public DirectionBombDestroyService(
            ILevelViewUpdater levelViewUpdater,
            IEntityDestroyable entityDestroyable,
            ILevelLoader levelLoader,
            SimpleDestroyService simpleDestroyService,
            IAnimatedDestroyService animatedDestroyService,
            IScreenInfoProvider screenInfoProvider,
            LaserEffect.Pool laserEffectPool,
            DestroyEntityEffectMapping destroyEntityEffectMapping,
            IKeyObjectPool<IEffect> keyObjectPool) : base(levelViewUpdater, destroyEntityEffectMapping, keyObjectPool)
        {
            _levelViewUpdater = levelViewUpdater;
            _entityDestroyable = entityDestroyable;
            _levelLoader = levelLoader;
            _simpleDestroyService = simpleDestroyService;
            _animatedDestroyService = animatedDestroyService;
            _screenInfoProvider = screenInfoProvider;
            _laserEffectPool = laserEffectPool;
        }

        public override async void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            NeedDestroy = true;
            GetDirection(entityView, out Direction firstDirection, out Direction secondDirection);

            int2 initialPoint = new int2(entityView.GridPositionX, entityView.GridPositionY);
            int2[] firstDirectionPoints = GetAllPointsByDirection(initialPoint, firstDirection);
            int2[] secondDirectionPoints = GetAllPointsByDirection(initialPoint, secondDirection);

            List<EntityData> firstEntityDatas = GetEntityDatas(firstDirectionPoints);
            List<EntityData> secondEntityDatas = GetEntityDatas(secondDirectionPoints);

            
            if (NeedDestroy == false)
                return;
            await PlayLasers(entityView);
            if (NeedDestroy == false)
                return;
            DestroyAll(new(gridItemData, entityView), firstEntityDatas, secondEntityDatas);
        }

        private async UniTask PlayLasers(IEntityView entityView)
        {
            LaserEffect firstLaser = _laserEffectPool.Spawn();
            LaserEffect secondLaser = _laserEffectPool.Spawn();

            firstLaser.transform.position = entityView.Position;
            secondLaser.transform.position = entityView.Position;
            
            ParticleSystem.MainModule firstMainModule = firstLaser.Laser.main;
            ParticleSystem.MainModule secondMainModule = secondLaser.Laser.main;

            if (entityView.BoostTypeId.Equals("HorizontalBomb"))
            {
                SetLasersAsHorizontal(entityView, firstMainModule, secondMainModule, firstLaser, secondLaser);
            }
            else
            {
                SetLasersAsVertical(entityView, firstMainModule, secondMainModule, firstLaser, secondLaser);
            }
            
            firstLaser.Laser.Play();
            secondLaser.Laser.Play();

            if (NeedDestroy == false)
            {
                return;
            }
            await UniTask.Delay(500);
                
            firstLaser.Laser.Stop();
            secondLaser.Laser.Stop();
                
            _laserEffectPool.Despawn(firstLaser);
            _laserEffectPool.Despawn(secondLaser);
        }

        private void SetLasersAsVertical(IEntityView entityView, ParticleSystem.MainModule firstMainModule, ParticleSystem.MainModule secondMainModule, LaserEffect firstLaser, LaserEffect secondLaser)
        {
            float screenHeight = _screenInfoProvider.HeightInWorld / 2f;

            UpdateRotation(firstMainModule, secondMainModule, 0f, 180f);
            
            if (entityView.Position.y > 0f)
            {
                var firstLaserTransform = firstLaser.transform;
                var position = firstLaserTransform.position;
                position = new(position.x, (-entityView.Position.y - screenHeight) / 2f + entityView.Position.y, position.z);

                firstLaserTransform.position = new Vector3(position.x, position.y, position.z);

                var secondLaserTransform = secondLaser.transform;
                var secondPosition = secondLaserTransform.position;
                secondPosition = new(position.x, (screenHeight - entityView.Position.y) / 2f + entityView.Position.y, position.z);

                secondLaserTransform.position = secondPosition;
                
                firstMainModule.startSizeY = screenHeight + entityView.Position.y;
                secondMainModule.startSizeY = screenHeight - entityView.Position.y;
            }
            else
            {
                var firstLaserTransform = firstLaser.transform;
                var position = firstLaserTransform.position;
                position = new(position.x, (Mathf.Abs(entityView.Position.y) + screenHeight) / 2f, position.z);

                firstLaserTransform.position = new Vector3(position.x, -position.y, position.z);

                var secondLaserTransform = secondLaser.transform;
                var secondPosition = secondLaserTransform.position;
                secondPosition = new(position.y, (screenHeight - Mathf.Abs(entityView.Position.y)) / 2f, position.z);

                secondLaserTransform.position = secondPosition;


                firstMainModule.startSizeY = screenHeight + entityView.Position.y;
                secondMainModule.startSizeY = screenHeight - entityView.Position.y;
            }
        }

        private void SetLasersAsHorizontal(IEntityView entityView, ParticleSystem.MainModule firstMainModule, ParticleSystem.MainModule secondMainModule, LaserEffect firstLaser, LaserEffect secondLaser)
        {
            float screenWidth = _screenInfoProvider.WidthInWorld / 2f;
            
            UpdateRotation(firstMainModule, secondMainModule, 90f, -90f);
            UpdateLasersHeight(entityView, firstMainModule, secondMainModule);

            if (entityView.Position.x > 0f)
            {
                var firstLaserTransform = firstLaser.transform;
                var position = firstLaserTransform.position;
                position = new((-entityView.Position.x - screenWidth) / 2f + entityView.Position.x, position.y, position.z);

                firstLaserTransform.position = new Vector3(position.x, position.y, position.z);

                var secondLaserTransform = secondLaser.transform;
                var secondPosition = secondLaserTransform.position;
                secondPosition = new((screenWidth - entityView.Position.x) / 2f + entityView.Position.x, position.y,
                    position.z);

                secondLaserTransform.position = secondPosition;


                firstMainModule.startSizeY = screenWidth + entityView.Position.x;
                secondMainModule.startSizeY = screenWidth - entityView.Position.x;
            }
            else
            {
                var firstLaserTransform = firstLaser.transform;
                var position = firstLaserTransform.position;
                position = new((Mathf.Abs(entityView.Position.x) + screenWidth) / 2f, position.y, position.z);

                firstLaserTransform.position = new Vector3(-position.x, position.y, position.z);

                var secondLaserTransform = secondLaser.transform;
                var secondPosition = secondLaserTransform.position;
                secondPosition = new((screenWidth - Mathf.Abs(entityView.Position.x)) / 2f, position.y, position.z);

                secondLaserTransform.position = secondPosition;


                firstMainModule.startSizeY = screenWidth + entityView.Position.x;
                secondMainModule.startSizeY = screenWidth - entityView.Position.x;
            }
        }

        private void UpdateRotation(ParticleSystem.MainModule firstMainModule, ParticleSystem.MainModule secondMainModule, float firstAngle, float secondAngle)
        {
            firstMainModule.startRotation = firstAngle * Mathf.Deg2Rad;
            secondMainModule.startRotation = secondAngle * Mathf.Deg2Rad;
        }

        private static void UpdateLasersHeight(IEntityView entityView, ParticleSystem.MainModule firstMainModule, ParticleSystem.MainModule secondMainModule)
        {
            var entityLocalScale = entityView.GameObject.transform.localScale;
            firstMainModule.startSizeX = entityLocalScale.x * 1.7786f;
            secondMainModule.startSizeX = entityLocalScale.x * 1.7786f;
        }

        private async void DestroyAll(EntityData currentBlock, List<EntityData> firstEntityDatas, List<EntityData> secondEntityDatas)
        {
            int maxSize = Math.Max(firstEntityDatas.Count, secondEntityDatas.Count);

            currentBlock.GridItemData.CurrentHealth = -1;

            await _animatedDestroyService.Animate(currentBlock.EntityView);
            _simpleDestroyService.Destroy(currentBlock.GridItemData, currentBlock.EntityView);
            
            for (int i = 0; i < maxSize; i++)
            {
                if (i < firstEntityDatas.Count)
                {
                    if (firstEntityDatas[i].GridItemData.CurrentHealth != -1)
                    {
                        firstEntityDatas[i].GridItemData.CurrentHealth = -1;
                        _entityDestroyable.Destroy(firstEntityDatas[i].GridItemData, firstEntityDatas[i].EntityView);

                        SetExplosionsEffect(firstEntityDatas[i].EntityView);
                    }
                }
                
                if (i < secondEntityDatas.Count)
                {
                    secondEntityDatas[i].GridItemData.CurrentHealth = -1;
                    _entityDestroyable.Destroy(secondEntityDatas[i].GridItemData, secondEntityDatas[i].EntityView);

                    SetExplosionsEffect(secondEntityDatas[i].EntityView);
                }

                if (NeedDestroy == false)
                    return;

                await UniTask.Delay(150);
                if (NeedDestroy == false)
                    return;
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
                
                result.Add(new()
                {
                    GridItemData = gridItemData,
                    EntityView = entityView
                });
            }

            return result;
        }

        private void GetDirection(IEntityView entityView, out Direction firstDirection, out Direction secondDirection)
        {
            if (entityView.BoostTypeId.Equals("HorizontalBomb"))
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