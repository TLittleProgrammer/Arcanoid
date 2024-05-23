﻿using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PlayerShape
{
    public interface IRectMousePositionChecker
    {
        bool MouseOnRect(Vector2 mousePosition);
    }
}