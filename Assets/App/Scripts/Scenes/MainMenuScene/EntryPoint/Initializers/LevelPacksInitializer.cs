using App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Installers
{
    public class LevelPacksInitializer : IInitializable
    {
        private readonly LevelPackParent _levelPackParent;
        private readonly LevelPackViewModel _levelPackViewModel;

        public LevelPacksInitializer(
        LevelPackParent levelPackParent,
        LevelPackViewModel levelPackViewModel)
        {
            _levelPackParent = levelPackParent;
            _levelPackViewModel = levelPackViewModel;
        }
        
        public void Initialize()
        {
            _levelPackViewModel.CreateAllLeveViewPacks(_levelPackParent);
        }
    }
}