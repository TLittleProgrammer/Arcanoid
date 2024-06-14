using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.PositionSystems;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird
{
    public class BirdsSystem : IBirdsService
    {
        private readonly IBirdHealthPointContainer _birdHealthPointContainer;
        private readonly IBirdPositionCheckerSystem _birdPositionCheckerSystem;
        private readonly IBirdMovementContainerSystem _birdMovementContainerSystem;

        private BirdRespawnService _birdRespawnService;

        public BirdsSystem(
            IBirdHealthPointContainer birdHealthPointContainer,
            BirdRespawnService birdRespawnService,
            IBirdPositionCheckerSystem birdPositionCheckerSystem,
            IBirdMovementContainerSystem birdMovementContainerSystem)
        {
            _birdRespawnService = birdRespawnService;
            _birdPositionCheckerSystem = birdPositionCheckerSystem;
            _birdMovementContainerSystem = birdMovementContainerSystem;
            _birdHealthPointContainer = birdHealthPointContainer;

            birdHealthPointContainer.BirdDied += Destroy;
            birdRespawnService.BirdRespawned += AddBird;
            birdRespawnService.BirdRespawned += EnableView;
        }

        public void AddBird(BirdView birdView)
        {
            _birdMovementContainerSystem.AddBird(birdView);
            
            IBirdPositionChecker birdPositionChecker = _birdPositionCheckerSystem.AddBird(birdView);

            birdPositionChecker.BirdFlewAway += OnBirdFlewAway;

            _birdHealthPointContainer.AddBird(birdView);
        }

        public void EnableView(BirdView birdView)
        {
            birdView.gameObject.SetActive(true);
            birdView.transform.localScale = Vector3.one;
        }

        public void Destroy(BirdView birdView)
        {
            birdView.transform.DOScale(Vector3.zero, 0.25f);

            _birdMovementContainerSystem.RemoveBird(birdView);
            _birdPositionCheckerSystem.RemoveBird(birdView);
        }

        public void StopAll()
        {
            _birdMovementContainerSystem.StopAll();
        }

        private void OnBirdFlewAway(BirdView view)
        {
            _birdMovementContainerSystem.RemoveBird(view);
            _birdPositionCheckerSystem.RemoveBird(view);
            _birdRespawnService.AddBirdToRespawn(view);
        }

        public void Restart()
        {
            _birdMovementContainerSystem.Restart();
        }
    }
}