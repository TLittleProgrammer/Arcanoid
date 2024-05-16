using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Components;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace App.Scripts.General.Popup
{
    public interface IPopupView : IGameObjectable
    {
        List<Button> Buttons { get; }
        
        UniTask Show();
        UniTask Close();
    }
}