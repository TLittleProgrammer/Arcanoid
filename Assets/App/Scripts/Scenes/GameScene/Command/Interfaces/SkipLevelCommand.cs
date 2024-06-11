using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Levels.SkipLevel;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Command.Interfaces
{
    public sealed class SkipLevelCommand : ISkipLevelCommand
    {
        private readonly ISkipLevelService _skipLevelService;
        private readonly IPopupService _popupService;

        public SkipLevelCommand(ISkipLevelService skipLevelService, IPopupService popupService)
        {
            _skipLevelService = skipLevelService;
            _popupService = popupService;
        }
        
        public async void Execute()
        {
            await UniTask.Delay(200);
            await _popupService.CloseAll();

            _skipLevelService.Skip();
        }
    }
}