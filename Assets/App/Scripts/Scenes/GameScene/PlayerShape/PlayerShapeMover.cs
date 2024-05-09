using App.Scripts.Scenes.GameScene.Input;
using App.Scripts.Scenes.GameScene.Time;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.PlayerShape
{
    public sealed class PlayerShapeMover : ITickable
    {
        private readonly ITransformable _playerTransformable;
        private readonly ITimeProvider _timeProvider;
        private readonly IInputService _inputService;
        
        private ShapeMoverSettings _shapeMoverSettings;

        public PlayerShapeMover(ITransformable playerTransformable, ITimeProvider timeProvider, IInputService inputService, ShapeMoverSettings shapeMoverSettings)
        {
            _playerTransformable = playerTransformable;
            _timeProvider = timeProvider;
            _inputService = inputService;
            _shapeMoverSettings = shapeMoverSettings;
        }
        
        public void Tick()
        {
            if (_inputService.UserClickDown)
            {
                _playerTransformable.Position = Vector2.Lerp
                    (
                        _playerTransformable.Position,
                        new(_inputService.CurrentMousePosition.x, _playerTransformable.Position.y),
                        _timeProvider.DeltaTime * _shapeMoverSettings.Speed
                    );
            }
        }
    }
}