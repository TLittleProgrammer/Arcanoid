using System;
using App.Scripts.Scenes.GameScene.Components;
using UnityEngine.EventSystems;

namespace App.Scripts.General.Components
{
    public interface IClickable : IComponent, IPointerClickHandler
    {
        event Action Clicked;
    }
}