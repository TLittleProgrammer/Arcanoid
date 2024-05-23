using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Ball;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.LevelProgress;
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

        public BombDestroyService(
            ILevelViewUpdater levelViewUpdater,
            ILevelLoader levelLoader,
            ILevelProgressService levelProgressService,
            IPoolContainer poolContainer,
            IBallSpeedUpdater ballSpeedUpdater)
        {
            _levelViewUpdater = levelViewUpdater;
            _levelLoader = levelLoader;
            _levelProgressService = levelProgressService;
            _poolContainer = poolContainer;
            _ballSpeedUpdater = ballSpeedUpdater;
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

            List<EntityData> immediateDatas = GetEntityDatas(simpleCorrectGridPosition);
            List<EntityData> diagonalsDatas = GetEntityDatas(diagonalsCorrectGridPosition);

            await AnimateAll(entityView, immediateDatas, diagonalsDatas);

            DestroyAllEntites(new()
            {
                EntityView = entityView,
                GridItemData = gridItemData
            }, immediateDatas, diagonalsDatas);
        }

        private async UniTask AnimateAll(EntityView bomb, List<EntityData> immediateEntityDatas, List<EntityData> diagonalsEntityDatas)
        {
            bomb.GameObject.transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InOutBounce).ToUniTask().Forget();
            await UniTask.Delay(350);
            
            foreach (EntityData data in immediateEntityDatas)
            {
                Transform transform = data.EntityView.GameObject.transform;

                transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InOutBounce).ToUniTask().Forget();
            }

            await UniTask.Delay(350);
            
            foreach (EntityData data in diagonalsEntityDatas)
            {
                Transform transform = data.EntityView.GameObject.transform;

                transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InOutBounce).ToUniTask().Forget();
            }

            await UniTask.Delay(350);
        }

        private void DestroyAllEntites(EntityData bomb, List<EntityData> immediateDatas, List<EntityData> diagonalDatas)
        {
            _levelProgressService.TakeOneStep();

            _poolContainer.RemoveItem(PoolTypeId.EntityView, bomb.EntityView as EntityView);
            foreach (OnTopSprites sprite in bomb.GridItemData.Sprites)
            {
                _poolContainer.RemoveItem(PoolTypeId.OnTopSprite, sprite);
            }

            _ballSpeedUpdater.UpdateSpeed();
            
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

            foreach (EntityData entityData in diagonalDatas)
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

        private List<EntityData> GetEntityDatas(int2[] simpleCorrectGridPosition)
        {
            List<EntityData> result = new();
            
            foreach (int2 position in simpleCorrectGridPosition)
            {
                GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[new Vector2Int(position.x, position.y)];
                IEntityView entityView = _levelLoader.Entities.First(x => x.GridPositionX == position.x && x.GridPositionY == position.y);

                entityView.BoxCollider2D.enabled = false;
                
                result.Add(new()
                {
                    GridItemData = gridItemData,
                    EntityView = entityView
                });
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