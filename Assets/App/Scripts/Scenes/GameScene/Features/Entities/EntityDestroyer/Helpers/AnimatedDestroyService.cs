using App.Scripts.Scenes.GameScene.Features.Entities.View;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers
{
    public class AnimatedDestroyService : IAnimatedDestroyService
    {
        public async UniTask Animate(IEntityView entityView)
        {
            Transform transform = entityView.GameObject.transform;

            transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InBack).ToUniTask().Forget();
            
            await UniTask.Delay(350);
        }
    }
}