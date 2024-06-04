using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies
{
    public sealed class GetBottomUsableItemService : IGetBottomUsableItemService
    {
        private readonly IBallsService _ballsService;
        private readonly IBoostMoveService _boostMoveService;

        public GetBottomUsableItemService(
            IBallsService ballsService,
            IBoostMoveService boostMoveService)
        {
            _ballsService = ballsService;
            _boostMoveService = boostMoveService;
        }
        
        public Vector3 GetBottomPosition()
        {
            Vector3 position = Vector3.one * 999f;
            
            CheckBallPositions(ref position);
            CheckBoostPositions(ref position);

            return position;
        }

        private void CheckBallPositions(ref Vector3 position)
        {
            foreach ((BallView ballView, IBallMovementService movementService) in _ballsService.Balls)
            {
                if (ballView.gameObject.activeSelf && ballView.transform.position.y < position.y && movementService.IsFreeFlight)
                {
                    position = ballView.transform.position;
                }
            }
        }

        private void CheckBoostPositions(ref Vector3 position)
        {
            Vector3 boostBottomPosition = Vector3.one * 999f;
            
            foreach (BoostView view in _boostMoveService.Views)
            {
                if (view.BoostTypeId.IsPositiveBoost() && view.gameObject.activeSelf && view.transform.position.y < boostBottomPosition.y)
                {
                    boostBottomPosition = view.transform.position;
                }
            }

            if (boostBottomPosition.y < position.y && position.y - boostBottomPosition.y >= 1f)
            {
                position = boostBottomPosition;
            }
        }
    }
}