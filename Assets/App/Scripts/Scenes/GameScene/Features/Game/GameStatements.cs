using System;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Game
{
    public class GameStatements : MonoBehaviour, IGameStatements
    {
        public event Action ApplicationQuit;

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                ApplicationQuit?.Invoke();
            }
        }
    }
}