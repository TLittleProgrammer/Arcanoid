using App.Scripts.Scenes.MainMenuScene.Features.LevelPacks.MonoBehaviours;
using App.Scripts.Scenes.MainMenuScene.MVVM.LevelPacks;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.EntryPoint.Initializers
{
    public class LevelPacksInitializer : IInitializable
    {
        private readonly LevelPackParent _levelPackParent;
        private readonly LevelPackViewModel _levelPackViewModel;

        public LevelPacksInitializer(LevelPackParent levelPackParent, LevelPackViewModel levelPackViewModel)
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