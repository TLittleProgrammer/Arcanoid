using System.Collections.Generic;
using App.Scripts.General.Components;
using App.Scripts.Scenes.GameScene.ScreenInfo;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PlayerShape
{
    public sealed class RectMousePositionChecker : IRectMousePositionChecker
    {
        private readonly IEnumerable<IRectTransformable> _rects;
        private readonly IScreenInfoProvider _screenInfoProvider;

        public RectMousePositionChecker(IEnumerable<IRectTransformable> rects, IScreenInfoProvider screenInfoProvider)
        {
            _rects = rects;
            _screenInfoProvider = screenInfoProvider;
        }        
        
        public bool MouseOnRect(Vector2 mousePosition)
        {
            foreach (IRectTransformable rect in _rects)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rect.RectTransform, 
                    mousePosition, 
                    null, 
                    out Vector2 localPoint
                );

                if (rect.RectTransform.rect.Contains(localPoint))
                {
                    return false;
                }
            }

            return true;
        }
    }
}