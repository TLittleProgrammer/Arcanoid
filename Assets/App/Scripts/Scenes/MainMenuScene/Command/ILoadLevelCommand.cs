using App.Scripts.General.Components;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours;

namespace App.Scripts.Scenes.MainMenuScene.Command
{
    public interface ILoadLevelCommand : ICommand<LevelItemData, int>
    {
        
    }
}