using System.Collections.Generic;
using App.Scripts.External.Extensions.ListExtensions;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Levels.General.Animations
{
    public sealed class ShowLevelService : IShowLevelService
    {
        private readonly List<IShowLevelAnimation> _levelAnimations;

        public ShowLevelService(List<IShowLevelAnimation> levelAnimations)
        {
            _levelAnimations = levelAnimations;
        }
        
        public UniTask Show()
        {
            return _levelAnimations.GetRandomValue().Show();
        }
    }
}