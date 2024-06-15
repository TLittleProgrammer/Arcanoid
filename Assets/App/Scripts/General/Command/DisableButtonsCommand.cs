using System.Collections.Generic;
using App.Scripts.General.AnimatableButtons;
using UnityEngine.UI;

namespace App.Scripts.General.Command
{
    public sealed class DisableButtonsCommand : IDisableButtonsCommand
    {
        public void Execute(List<Button> buttons)
        {
            foreach (Button button in buttons)
            {
                button.enabled = false;

                TryAnimationScriptDisable(button);
            }
        }

        private void TryAnimationScriptDisable(Button button)
        {
            if (button.TryGetComponent(out DotweenAnimatableButton animatableButton))
            {
                animatableButton.enabled = false;
            }
        }
    }
}