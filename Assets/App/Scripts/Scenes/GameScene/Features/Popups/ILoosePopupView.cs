using App.Scripts.External.Components;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public interface ILoosePopupView : IGameObjectable
    {
        Button RestartButton { get; }
    }
}