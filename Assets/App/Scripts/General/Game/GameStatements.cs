using System;
using UnityEngine;

namespace App.Scripts.General.Game
{
    public class GameStatements : MonoBehaviour, IGameStatements
    {
        public event Action ApplicationQuit;
        public event Action<bool> ApplicationFocus;

        private void OnApplicationFocus(bool hasFocus)
        {
            ApplicationFocus?.Invoke(hasFocus);
            
            if (!hasFocus)
            {
                ApplicationQuit?.Invoke();
            }
        }
    }
}