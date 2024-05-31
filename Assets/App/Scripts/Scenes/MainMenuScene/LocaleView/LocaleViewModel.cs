using UnityEngine;

namespace App.Scripts.Scenes.MainMenuScene.LocaleView
{
    public record LocaleViewModel
    {
        public Sprite Sprite;
        public string LocaleToken;
        public string LocaleTokenText;
        public string LocaleKey;
    }
}