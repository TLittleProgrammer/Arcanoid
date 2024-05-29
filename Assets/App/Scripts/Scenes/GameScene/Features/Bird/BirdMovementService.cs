using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Bird
{
    public class BirdMovementService : IBirdMovement
    {
        private readonly ITransformable _bird;
        private readonly BirdSettings _birdSettings;
        private readonly ITimeProvider _timeProvider;

        private float _allTime = 0f;
        private Vector2 _direction;

        public BirdMovementService(ITransformable bird, BirdSettings birdSettings, ITimeProvider timeProvider)
        {
            _bird = bird;
            _birdSettings = birdSettings;
            _timeProvider = timeProvider;
        }

        public bool IsActive { get; set; }

        public async UniTask AsyncInitialize(Vector2 direction)
        {
            _direction = direction;

            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (!IsActive)
                return;

            Fly();
        }

        private void Fly()
        {
            _allTime += _timeProvider.DeltaTime;

            float positionY = Mathf.Sin(_allTime) * _birdSettings.Amplitude;
            float positionX = _bird.Transform.position.x + _direction.x * _birdSettings.HorizontalSpeed * _timeProvider.DeltaTime;

            _bird.Transform.position = new Vector3(positionX, positionY, 0f);
        }
    }
}