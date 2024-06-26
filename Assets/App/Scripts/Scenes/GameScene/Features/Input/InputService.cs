﻿using App.Scripts.Scenes.GameScene.Features.Camera;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Input
{
    public sealed class InputService : IInputService
    {
        private readonly ICameraService _cameraService;
        
        private bool _userClickOnScreenDown;
        private Vector2 _currentMousePosition;

        public InputService(IClickDetector clickDetector, ICameraService cameraService)
        {
            _cameraService = cameraService;
            clickDetector.MouseDowned += OnMouseDowned;
            clickDetector.MouseUp     += OnMouseUp;
        }

        public void Tick()
        {
            if (_userClickOnScreenDown)
            {
                Vector2 mousePosition = UnityEngine.Input.mousePosition;

                if (!_currentMousePosition.Equals(mousePosition))
                {
                    _currentMousePosition = mousePosition;
                    CurrentMouseWorldPosition = _cameraService.ScreenToWorldPoint(_currentMousePosition);
                }
            }
        }

        public bool UserClickDown => _userClickOnScreenDown;
        public Vector2 CurrentMouseWorldPosition { get; private set; }

        private void OnMouseUp()
        {
            _userClickOnScreenDown = false;
        }

        private void OnMouseDowned()
        {
            _userClickOnScreenDown = true;
        }
    }
}