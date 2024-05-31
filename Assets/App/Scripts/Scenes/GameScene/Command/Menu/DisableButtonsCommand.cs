using System.Collections.Generic;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Command
{
    public sealed class DisableButtonsCommand : IDisableButtonsCommand
    {
        public void Execute(List<Button> buttons)
        {
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }
    }
}