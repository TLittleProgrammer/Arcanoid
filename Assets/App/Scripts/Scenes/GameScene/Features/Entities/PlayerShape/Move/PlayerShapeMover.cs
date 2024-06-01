using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Input;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Time;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move
{
    public sealed class PlayerShapeMover : IPlayerShapeMover, ILevelProgressSavable, IInitializeByLevelProgress
    {
        private readonly IPositionable _playerPositionable;
        private readonly ITimeProvider _timeProvider;
        private readonly IInputService _inputService;
        private readonly IShapePositionChecker _positionChecker;

        private ShapeMoverSettings _shapeMoverSettings;
        private readonly IRectMousePositionChecker _rectMousePositionChecker;
        private Vector3 _initialPosition;
        private float _speed;

        public PlayerShapeMover(
            IPositionable playerPositionable,
            ITimeProvider timeProvider,
            IInputService inputService,
            IShapePositionChecker positionChecker,
            ShapeMoverSettings shapeMoverSettings,
            IRectMousePositionChecker rectMousePositionChecker)
        {
            _playerPositionable = playerPositionable;
            _timeProvider = timeProvider;
            _inputService = inputService;
            _positionChecker = positionChecker;
            _shapeMoverSettings = shapeMoverSettings;
            _rectMousePositionChecker = rectMousePositionChecker;
            _initialPosition = _playerPositionable.Position;

            _speed = _shapeMoverSettings.Speed;
            IsActive = true;
        }

        public bool IsActive { get; set; }

        public void Tick()
        {
            if (!IsActive)
                return;
            
            if (_inputService.UserClickDown && _rectMousePositionChecker.MouseOnRect(_inputService.CurrentMousePosition))
            {
                Vector2 targetPosition = CalculateTargetPosition();

                TryMoveShape(targetPosition);
            }
        }

        private void TryMoveShape(Vector2 targetPosition)
        {
            if (_positionChecker.CanChangePositionTo(targetPosition))
            {
                _playerPositionable.Position = targetPosition;
            }
            else
            {
                float targetXPosition = _playerPositionable.Position.x < 0f ? _positionChecker.MinX : _positionChecker.MaxX;
                
                _playerPositionable.Position = new Vector3(
                    targetXPosition,
                    _playerPositionable.Position.y,
                    0f
                );
            }
        }

        private Vector2 CalculateTargetPosition()
        {
            return Vector2.MoveTowards
            (
                _playerPositionable.Position,
                new(_inputService.CurrentMousePosition.x, _playerPositionable.Position.y),
                _timeProvider.DeltaTime * _speed
            );
        }

        public void Restart()
        {
            _playerPositionable.Position = _initialPosition;
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public void ChangeSpeed(float speedScale)
        {
            _speed = _shapeMoverSettings.Speed * speedScale;
        }

        public void SaveProgress(LevelDataProgress levelDataProgress)
        {
            PlatformSaveData platformSaveData = new();
            platformSaveData.Position = new()
            {
                X = _playerPositionable.Position.x,
                Y = _playerPositionable.Position.y,
                Z = _playerPositionable.Position.z,
            };

            levelDataProgress.PlatformData = platformSaveData;
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            Vector3 targetPosition = new(
                levelDataProgress.PlatformData.Position.X,
                levelDataProgress.PlatformData.Position.Y,
                levelDataProgress.PlatformData.Position.Z
                );
            
            _playerPositionable.Position = targetPosition;
        }
    }
}