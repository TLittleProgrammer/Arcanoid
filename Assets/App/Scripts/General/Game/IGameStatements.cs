using System;

namespace App.Scripts.General.Game
{
    public interface IGameStatements
    {
        event Action ApplicationQuit;
        event Action<bool> ApplicationFocus;
    }
}