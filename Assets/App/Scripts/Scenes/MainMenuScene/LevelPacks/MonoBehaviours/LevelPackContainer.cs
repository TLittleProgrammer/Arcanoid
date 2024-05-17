using App.Scripts.Scenes.MainMenuScene.Factories.Levels;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours
{
    public class LevelPackContainer : MonoBehaviour
    {
        [Inject]
        private void Construct(LevelPackProvider levelPackProvider, ILevelItemView.Factory levelItemFactory)
        {
            CreateAllLevelPacks(levelPackProvider, levelItemFactory);
        }

        private void CreateAllLevelPacks(LevelPackProvider levelPackProvider, ILevelItemView.Factory levelItemFactory)
        {
            for (int i = 0; i < levelPackProvider.LevelPacks.Count; i++)
            {
                levelItemFactory.Create(i, levelPackProvider.LevelPacks[i]);
            }
        }
    }
}