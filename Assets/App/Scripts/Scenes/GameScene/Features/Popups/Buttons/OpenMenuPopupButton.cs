using System;
using App.Scripts.External.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Scripts.Scenes.GameScene.Features.Popups.Buttons
{
    public class OpenMenuPopupButton : MonoBehaviour, IClickable
    {
        public event Action Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }
    }
}