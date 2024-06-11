using App.Scripts.External.Components;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public interface IMenuPopupView : ITransformable
    {
        Button RestartButton { get; }
        Button BackButton { get; }
        Button ContinueButton { get; }
        Button SkipLevelButton { get; }
    }
}