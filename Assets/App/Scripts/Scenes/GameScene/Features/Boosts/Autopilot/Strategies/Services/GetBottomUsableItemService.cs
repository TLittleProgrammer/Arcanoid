using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
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
            foreach ((BallView ballView, var garbarge) in _ballsService.Balls)
            {
                if (ballView.gameObject.activeSelf && ballView.transform.position.y < position.y)
                {
                    position = ballView.transform.position;
                }
            }
        }

        private void CheckBoostPositions(ref Vector3 position)
        {
            foreach (BoostView view in _boostMoveService.Views)
            {
                if (view.BoostTypeId.IsPositiveBoost() && view.gameObject.activeSelf && view.transform.position.y < position.y)
                {
                    position = view.transform.position;
                }
            }
        }
    }
}