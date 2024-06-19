using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Factory;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird
{
    public class BirdMovementContainerSystem : IBirdMovementContainerSystem
    {
        private readonly IBirdMovementSystemFactory _birdMovementSystemFactory;
        private readonly IScreenInfoProvider _screenInfoProvider;
        private readonly Dictionary<BirdView, IBirdMovement> _birds = new();

        private const float SpawnOffset = 1f;

        public BirdMovementContainerSystem(IBirdMovementSystemFactory birdMovementSystemFactory, IScreenInfoProvider screenInfoProvider)
        {
            _birdMovementSystemFactory = birdMovementSystemFactory;
            _screenInfoProvider = screenInfoProvider;
        }

        public bool IsActive { get; set; }

        public void Tick()
        {
            if (IsActive == false)
                return;

            FlyAll();
        }

        private void FlyAll()
        {
            foreach ((BirdView view, IBirdMovement movement) in _birds)
            {
                movement.Tick();
            }
        }

        public void AddBird(BirdView birdView)
        {
            if (_birds.ContainsKey(birdView))
            {
                _birds[birdView].IsActive = true;
            }
            else
            {
                AddBirdToDictionary(birdView);
            }

            _birds[birdView].Direction = Random.Range(0, 2) == 0 ? Direction.Right : Direction.Left;
            SetInitialSettings(birdView);
        }

        public void RemoveBird(BirdView birdView)
        {
            if (_birds.ContainsKey(birdView))
            {
                _birds[birdView].IsActive = false;
            }
        }

        public void StopAll()
        {
            foreach ((BirdView view, IBirdMovement movement) in _birds)
            {
                movement.IsActive = false;
            }
        }

        public void Restart()
        {
            foreach ((BirdView view, IBirdMovement movement) in _birds)
            {
                if (movement.IsActive)
                {
                    float xPosition = ChooseXPosition(view);
                    
                    SetBirdXPosition(view, xPosition);
                }
            }
        }

        private void SetInitialSettings(BirdView birdView)
        {
            Direction currentDirection = _birds[birdView].Direction;

            if (currentDirection == Direction.Left)
            {
                InitializeBird(birdView, Direction.Right, true);
            }
            else
            {
                InitializeBird(birdView, Direction.Left, false);
            }
        }

        private void InitializeBird(BirdView birdView, Direction direction, bool flip)
        {
            birdView.Transform.position = new Vector3(ChooseXPosition(birdView), 0f, 0f);
            birdView.SpriteRenderer.flipX = flip;

            _birds[birdView].Direction = direction;
        }

        private float ChooseXPosition(BirdView view)
        {
            return _birds[view].Direction == Direction.Right
                ? _screenInfoProvider.WidthInWorld / 2f + SpawnOffset
                : -_screenInfoProvider.WidthInWorld / 2f - SpawnOffset;
        }
        
        private void SetBirdXPosition(BirdView birdView, float x)
        {
            birdView.transform.position = new(x, 0f, 0f);
        }

        private void AddBirdToDictionary(BirdView birdView)
        {
            IBirdMovement movement = _birdMovementSystemFactory.Create(birdView);
            movement.IsActive = true;
            
            _birds.Add(birdView, movement);
        }
    }
}