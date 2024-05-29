using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird
{
    public class BirdsService : IBirdsService, ITickable
    {
        private readonly IScreenInfoProvider _screenInfoProvider;
        private readonly BirdView.Pool _birdViewPool;
        private readonly IBirdHealthPointContainer _birdHealthPointContainer;
        private readonly Dictionary<BirdView, Direction> _lastDirection;

        public Dictionary<BirdView, IBirdMovement> Birds { get; private set; }

        private List<IBirdPositionChecker> _birdPositionCheckers;
        private BirdRespawnService _birdRespawnService;

        public BirdsService(
            IScreenInfoProvider screenInfoProvider,
            BirdView.Pool birdViewPool,
            IBirdHealthPointContainer birdHealthPointContainer,
            BirdRespawnService birdRespawnService)
        {
            _birdRespawnService = birdRespawnService;
            _screenInfoProvider = screenInfoProvider;
            _birdViewPool = birdViewPool;
            _birdHealthPointContainer = birdHealthPointContainer;
            _lastDirection = new();
            Birds = new();
            _birdPositionCheckers = new();

            birdHealthPointContainer.BirdDied += Destroy;
            birdRespawnService.BirdRespawned += AddBird;
            birdRespawnService.BirdRespawned += GoFly;
        }

        public bool IsActive { get; set; }

        public void Tick()
        {
            if (!IsActive)
                return;
            
            foreach ((BirdView view, IBirdMovement movement) in Birds)
            {
                movement.Tick();
            }
            
            foreach (IBirdPositionChecker positionChecker in _birdPositionCheckers)
            {
                positionChecker.Tick();
            }
        }

        public void AddBird(BirdView birdView)
        {
            IBirdPositionChecker birdPositionChecker = new BirdPositionChecker(birdView, _screenInfoProvider);
            birdPositionChecker.BirdFlewAway += OnBirdFlewAway;
            
            _birdPositionCheckers.Add(birdPositionChecker);
            Birds[birdView].IsActive = false;
            
            _birdHealthPointContainer.AddBird(birdView);
        }

        public void GoFly(BirdView birdView)
        {
            if (!_lastDirection.ContainsKey(birdView))
            {
                _lastDirection.Add(birdView, Direction.Right);
                UpdateBirdParameters(birdView, -_screenInfoProvider.WidthInWorld / 2f - 1f, false, Direction.Right);
            }

            if (_lastDirection[birdView] == Direction.Right)
            {
                UpdateBirdParameters(birdView, _screenInfoProvider.WidthInWorld / 2f + 1f, false, Direction.Left);
            }
            else
            {
                UpdateBirdParameters(birdView, -_screenInfoProvider.WidthInWorld / 2f - 1f, true, Direction.Right);
            }

            Vector2 direction = new Vector2(_lastDirection[birdView].ToVector().x, 0f);
            
            Birds[birdView].AsyncInitialize(direction);
            Birds[birdView].IsActive = true;
            
            birdView.gameObject.SetActive(true);
            birdView.transform.localScale = Vector3.one;
        }

        public void Destroy(BirdView birdView)
        {
            Birds[birdView].IsActive = false;
            birdView.transform.DOScale(Vector3.zero, 0.25f);

            IBirdPositionChecker birdPositionChecker = _birdPositionCheckers.First(x => x.BirdView.Equals(birdView));
            _birdPositionCheckers.Remove(birdPositionChecker);
        }

        public void StopAll()
        {
            foreach ((BirdView view, IBirdMovement movement) in Birds)
            {
                movement.IsActive = false;
            }
        }

        private async void OnBirdFlewAway(BirdView view, IBirdPositionChecker birdPositionChecker)
        {
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);
            Birds[view].IsActive = false;
            _birdRespawnService.AddBirdToRespawn(view);
            
            _birdPositionCheckers.Remove(birdPositionChecker);
        }

        private void UpdateBirdParameters(BirdView birdView, float xPosition, bool flip, Direction direction)
        {
            SetBirdXPosition(birdView, xPosition);
            birdView.SpriteRenderer.flipX = flip;

            _lastDirection[birdView] = direction;
        }

        private void SetBirdXPosition(BirdView birdView, float x)
        {
            birdView.transform.position = new(x, 0f, 0f);
        }

        public void Restart()
        {
            foreach ((BirdView view, IBirdMovement movement) in Birds)
            {
                if (movement.IsActive)
                {
                    if (_lastDirection[view] == Direction.Left)
                    {
                        SetBirdXPosition(view, _screenInfoProvider.WidthInWorld / 2f + 1f);
                    }
                    else
                    {
                        SetBirdXPosition(view, -_screenInfoProvider.WidthInWorld / 2f - 1f);
                    }
                }
            }
        }
    }
}