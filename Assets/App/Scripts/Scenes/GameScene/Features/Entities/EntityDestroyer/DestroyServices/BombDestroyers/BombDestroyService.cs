using System.Collections.Generic;
using System.Linq;
using System.Threading;
using App.Scripts.External.ObjectPool;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
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
    public sealed class BombDestroyService : BombDestroyer
    {
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly ILevelLoader _levelLoader;
        private readonly IAnimatedDestroyService _animatedDestroyService;
        private readonly IAddVisualDamage _addVisualDamage;
        private readonly IEntityDestroyable _entityDestroyable;
        private readonly SimpleDestroyService _simpleDestroyService;


        public BombDestroyService(
            ILevelViewUpdater levelViewUpdater,
            ILevelLoader levelLoader,
            IAnimatedDestroyService animatedDestroyService,
            IAddVisualDamage addVisualDamage,
            IEntityDestroyable entityDestroyable,
            SimpleDestroyService simpleDestroyService,
            DestroyEntityEffectMapping destroyEntityEffectMapping,
            IKeyObjectPool<IEffect> keyObjectPool) : base(levelViewUpdater, destroyEntityEffectMapping, keyObjectPool)
        {
            _levelViewUpdater = levelViewUpdater;
            _levelLoader = levelLoader;
            _animatedDestroyService = animatedDestroyService;
            _addVisualDamage = addVisualDamage;
            _entityDestroyable = entityDestroyable;
            _simpleDestroyService = simpleDestroyService;
        }

        public override async void Destroy(GridItemData gridItemData, IEntityView iEntityView)
        {
            NeedDestroy = true;
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
        }

        private async UniTask AnimateAll(EntityView bomb, List<EntityData> immediateEntityDatas, List<EntityData> diagonalsEntityDatas)
        {
            SetExplosionsEffect(bomb);
            GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[bomb.GridPositionX, bomb.GridPositionY];
            gridItemData.CurrentHealth = -1;
            
            await _animatedDestroyService.Animate(bomb);
            
            _simpleDestroyService.Destroy(gridItemData, bomb);

            foreach (EntityData data in immediateEntityDatas)
            {
                _entityDestroyable.Destroy(data.GridItemData, data.EntityView);
            }

            foreach (EntityData data in diagonalsEntityDatas)
            {
                _entityDestroyable.Destroy(data.GridItemData, data.EntityView);
            }

            if (NeedDestroy == false)
            {
                return;
            }
            await UniTask.Delay(350);
        }

        private List<EntityData> GetEntityDatas(int2[] simpleCorrectGridPosition, int damage)
        {
            List<EntityData> result = new();
            
            foreach (int2 position in simpleCorrectGridPosition)
            {
                GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[new Vector2Int(position.x, position.y)];
                IEntityView entityView = _levelLoader.Entities.FirstOrDefault(x => x.GridPositionX == position.x && x.GridPositionY == position.y);

                if (entityView is not null)
                {
                    SetExplosionsEffect(entityView);
                
                    if (gridItemData.CurrentHealth - damage <= 0 && entityView.BoxCollider2D.enabled)
                    {
                        entityView.BoxCollider2D.enabled = false;
                        gridItemData.CurrentHealth -= damage;
                        result.Add(new()
                        {
                            GridItemData = gridItemData,
                            EntityView = entityView
                        });
                        continue;
                    }

                    _addVisualDamage.AddVisualDamage(damage, gridItemData, entityView);
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