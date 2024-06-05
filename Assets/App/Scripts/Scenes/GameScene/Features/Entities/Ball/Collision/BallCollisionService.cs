using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Entities.Walls;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using App.Scripts.Scenes.GameScene.Features.Shake;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Collision
{
    public class BallCollisionService : IBallCollisionService
    {
        private readonly BallView _ball;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IShakeService _shakeService;
        private readonly PlazmaEffect.Pool _plazmaEffectPool;
        private readonly float _minBallYPosition;

        private readonly float _screenWidth;

        public BallCollisionService(
            BallView ball,
            ILevelViewUpdater levelViewUpdater,
            IShakeService shakeService,
            PlazmaEffect.Pool plazmaEffectPool,
            IScreenInfoProvider screenInfoProvider)
        {
            _ball = ball;
            _levelViewUpdater = levelViewUpdater;
            _shakeService = shakeService;
            _plazmaEffectPool = plazmaEffectPool;

            _screenWidth = screenInfoProvider.WidthInWorld / 2f;

            _ball.Collidered += OnCollidered;
        }

        private void OnCollidered(BallView view, Collider2D collider)
        {
            if (collider.TryGetComponent(out EntityView entityView))
            {
                _shakeService.Shake(entityView.transform);
                _levelViewUpdater.UpdateVisual(entityView, 1);
            }
            else if (collider.TryGetComponent(out WallView wallView))
            {
                PlayPlazmaEffect(wallView.transform.position.x, view.Position);
            }
        }

        private async void PlayPlazmaEffect(float positionX, Vector3 viewPosition)
        {
            PlazmaEffect plazmaEffect = _plazmaEffectPool.Spawn();
            ParticleSystem.MainModule mainModule = plazmaEffect.ParticleSystem.main;
            
            if (positionX > _screenWidth)
            {
                mainModule.startRotation = 180f * Mathf.Deg2Rad;
            }
            else if(positionX < -_screenWidth)
            {
                mainModule.startRotation = 0f;
            }
            else
            {
                mainModule.startRotation = 90f * Mathf.Deg2Rad;
            }

            plazmaEffect.transform.position = viewPosition;
            plazmaEffect.ParticleSystem.Play();
            
            await UniTask.Delay(1000);
            _plazmaEffectPool.Despawn(plazmaEffect);
        }
    }
}