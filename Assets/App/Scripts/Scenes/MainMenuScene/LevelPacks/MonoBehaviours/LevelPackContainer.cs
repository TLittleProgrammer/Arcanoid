using App.Scripts.Scenes.MainMenuScene.Factories.Levels;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours
{
    public class LevelPackContainer : MonoBehaviour
    {
        [SerializeField] private LevelItemView _levelItemViewPrefab;
        [SerializeField] private LevelPackParent _levelPackParent;
        
        [Inject]
        private void Construct(LevelPackProvider levelPackProvider, LevelItemFactory levelItemFactory)
        {
            CreateAllLevelPacks(levelPackProvider, levelItemFactory);
        }

        private void CreateAllLevelPacks(LevelPackProvider levelPackProvider, LevelItemFactory levelItemFactory)
        {
            for (int i = 0; i < levelPackProvider.LevelPacks.Count; i++)
            {
                levelItemFactory.Create(i, levelPackProvider.LevelPacks[i]);
            }
        }
    }
}