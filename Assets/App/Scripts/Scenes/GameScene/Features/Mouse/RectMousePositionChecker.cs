using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelView;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape
{
    public sealed class RectMousePositionChecker : IRectMousePositionChecker
    {
        private readonly List<RectTransformableView> _rects;
        
        public RectMousePositionChecker(List<RectTransformableView> rects)
        {
            _rects = rects;
        }        
        
        public bool MouseOnRect(Vector2 mousePosition)
        {
            foreach (IRectTransformable rect in _rects)
            {
                if (rect.RectTransform.RectContainsPosition(mousePosition))
                {
                    return true;
                }
            }

            return false;
        }
    }
}