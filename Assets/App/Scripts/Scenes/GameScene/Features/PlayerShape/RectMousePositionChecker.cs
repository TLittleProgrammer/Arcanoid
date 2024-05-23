using System.Collections.Generic;
using App.Scripts.External.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PlayerShape
{
    public sealed class RectMousePositionChecker : IRectMousePositionChecker
    {
        private readonly IEnumerable<IRectTransformable> _rects;

        public RectMousePositionChecker(IEnumerable<IRectTransformable> rects)
        {
            _rects = rects;
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