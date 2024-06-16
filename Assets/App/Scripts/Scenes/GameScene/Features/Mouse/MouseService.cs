using App.Scripts.Scenes.GameScene.Features.Input;

namespace App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape
{
    public sealed class MouseService : IMouseService
    {
        private readonly IInputService _inputService;
        private readonly IRectMousePositionChecker _rectMousePositionChecker;

        public MouseService(IInputService inputService, IRectMousePositionChecker rectMousePositionChecker)
        {
            _inputService = inputService;
            _rectMousePositionChecker = rectMousePositionChecker;
        }

        public bool IsMouseOnRect()
        {
            return _rectMousePositionChecker.MouseOnRect(_inputService.CurrentMouseWorldPosition);
        }
    }
}