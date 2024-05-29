using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Effects.Bombs;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Levels.Load;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices
{
    public class DirectionBombDestroyService : DestroyService
    {
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IItemsDestroyable _itemsDestroyable;
        private readonly ILevelLoader _levelLoader;
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly IAnimatedDestroyService _animatedDestroyService;
        private readonly IScreenInfoProvider _screenInfoProvider;
        private readonly LaserEffect.Pool _laserEffectPool;
        private readonly ExplosionEffect.Pool _explosionsPool;

        public DirectionBombDestroyService(
            ILevelViewUpdater levelViewUpdater,
            IItemsDestroyable itemsDestroyable,
            ILevelLoader levelLoader,
            SimpleDestroyService simpleDestroyService,
            IAnimatedDestroyService animatedDestroyService,
            IScreenInfoProvider screenInfoProvider,
            LaserEffect.Pool laserEffectPool,
            ExplosionEffect.Pool explosionsPool) : base(levelViewUpdater)
        {
            _levelViewUpdater = levelViewUpdater;
            _itemsDestroyable = itemsDestroyable;
            _levelLoader = levelLoader;
            _simpleDestroyService = simpleDestroyService;
            _animatedDestroyService = animatedDestroyService;
            _screenInfoProvider = screenInfoProvider;
            _laserEffectPool = laserEffectPool;
            _explosionsPool = explosionsPool;
        }
        
        public override async void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            GetDirection(entityView, out Direction firstDirection, out Direction secondDirection);

            int2 initialPoint = new int2(entityView.GridPositionX, entityView.GridPositionY);
            int2[] firstDirectionPoints = GetAllPointsByDirection(initialPoint, firstDirection);
            int2[] secondDirectionPoints = GetAllPointsByDirection(initialPoint, secondDirection);

            List<EntityData> firstEntityDatas = GetEntityDatas(firstDirectionPoints);
            List<EntityData> secondEntityDatas = GetEntityDatas(secondDirectionPoints);

            await PlayLasers(entityView);
            
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

            if (entityView.BoostTypeId is BoostTypeId.HorizontalBomb)
            {
                SetLasersAsHorizontal(entityView, firstMainModule, secondMainModule, firstLaser, secondLaser);
            }
            else
            {
                SetLasersAsVertical(entityView, firstMainModule, secondMainModule, firstLaser, secondLaser);
            }
            
            firstLaser.Laser.Play();
            secondLaser.Laser.Play();
                
            await UniTask.Delay(1000);
                
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

            await _animatedDestroyService.Animate(new List<EntityData> { currentBlock });
            _simpleDestroyService.Destroy(currentBlock.GridItemData, currentBlock.EntityView);
            
            for (int i = 0; i < maxSize; i++)
            {
                if (i < firstEntityDatas.Count)
                {
                    firstEntityDatas[i].GridItemData.CurrentHealth = -1;
                    _itemsDestroyable.Destroy(firstEntityDatas[i].GridItemData, firstEntityDatas[i].EntityView);
                    
                    ExplosionEffect effect = _explosionsPool.Spawn();
                    ParticleSystem.MainModule explosionMain = effect.Explosion.main;
                    explosionMain.startSizeX = firstEntityDatas[i].EntityView.GameObject.transform.localScale.x * 1.431f;

                    effect.transform.position = firstEntityDatas[i].EntityView.Position;
                    effect.Explosion.Play();
                }
                
                if (i < secondEntityDatas.Count)
                {
                    secondEntityDatas[i].GridItemData.CurrentHealth = -1;
                    _itemsDestroyable.Destroy(secondEntityDatas[i].GridItemData, secondEntityDatas[i].EntityView);
                    
                    ExplosionEffect effect = _explosionsPool.Spawn();
                    ParticleSystem.MainModule explosionMain = effect.Explosion.main;
                    explosionMain.startSizeX = secondEntityDatas[i].EntityView.GameObject.transform.localScale.x * 1.431f;

                    effect.transform.position = secondEntityDatas[i].EntityView.Position;
                    effect.Explosion.Play();
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