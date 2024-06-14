using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Time;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird
{
    public class BirdMovementSystem : IBirdMovement
    {
        private readonly ITransformable _bird;
        private readonly BirdSettings _birdSettings;
        private readonly ITimeProvider _timeProvider;

        private float _allTime;
        private Vector2 _direction;

        public BirdMovementSystem(
            ITransformable bird, 
            BirdSettings birdSettings, 
            ITimeProvider timeProvider)
        {
            _bird = bird;
            _birdSettings = birdSettings;
            _timeProvider = timeProvider;
        }
        
        public bool IsActive { get; set; }

        public Direction Direction
        {
            get => _direction.x < 0f ? Direction.Left : Direction.Right;
            set
            {
                int2 vector = value.ToVector();

                _direction = new(vector.x, vector.y);
            }
        }

        public void Tick()
        {
            if (IsActive == false)
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