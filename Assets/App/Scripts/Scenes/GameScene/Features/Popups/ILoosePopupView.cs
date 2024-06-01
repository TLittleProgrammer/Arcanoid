using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public interface ILoosePopupView
    {
        Button RestartButton { get; }
    }
}