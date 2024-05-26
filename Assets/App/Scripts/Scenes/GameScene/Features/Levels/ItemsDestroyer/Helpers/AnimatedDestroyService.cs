using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers
{
    public class AnimatedDestroyService : IAnimatedDestroyService
    {
        public async UniTask Animate(List<EntityData> immediateEntityDatas)
        {
            foreach (EntityData data in immediateEntityDatas)
            {
                Transform transform = data.EntityView.GameObject.transform;

                transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InBack).ToUniTask().Forget();
            }

            await UniTask.Delay(350);
        }

        public async UniTask Animate(IEntityView entityView)
        {
            Transform transform = entityView.GameObject.transform;

            transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InBack).ToUniTask().Forget();
            
            await UniTask.Delay(350);
        }
    }
}