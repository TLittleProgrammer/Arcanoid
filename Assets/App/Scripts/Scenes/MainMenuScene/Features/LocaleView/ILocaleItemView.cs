using App.Scripts.External.Components;

namespace App.Scripts.Scenes.MainMenuScene.LocaleView
{
    public interface ILocaleItemView : IGameObjectable
    {
        void SetModel(LocaleViewModel model);
    }
}