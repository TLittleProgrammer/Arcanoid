using App.Scripts.Scenes.GameScene.Features.Levels;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene
{
    public class SaveLevel : MonoBehaviour
    {
        private LevelProgressSaveService _levelProgressSaveService;

        [Inject]
        private void Construct(LevelProgressSaveService levelProgressSaveService)
        {
            _levelProgressSaveService = levelProgressSaveService;
        }
        
        [Button("Save")]
        public void Save()
        {
            _levelProgressSaveService.SaveProgress();
        }
    }
}