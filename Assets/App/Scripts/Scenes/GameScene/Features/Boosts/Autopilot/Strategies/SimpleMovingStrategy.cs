using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Nodes;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies
{
    public class SimpleMovingStrategy : IStrategy
    {
        private readonly IGetBottomUsableItemService _getBottomUsableItemService;
        private readonly IAutopilotMoveService _autopilotMoveService;

        public SimpleMovingStrategy(IGetBottomUsableItemService getBottomUsableItemService, IAutopilotMoveService autopilotMoveService)
        {
            _getBottomUsableItemService = getBottomUsableItemService;
            _autopilotMoveService = autopilotMoveService;
        }
        
        public NodeStatus Process()
        {
            Vector3 bottomUsablePosition = _getBottomUsableItemService.GetBottomPosition();

            _autopilotMoveService.Move(bottomUsablePosition);

            return NodeStatus.Success;
        }
    }
}