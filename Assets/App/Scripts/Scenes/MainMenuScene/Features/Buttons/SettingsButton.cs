using App.Scripts.General.Components;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenuScene.Features.Buttons
{
    public class SettingsButton : MonoBehaviour, IButtonable
    {
        [SerializeField] private Button _button;

        public Button Button => _button;
    }
}