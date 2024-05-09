using App.Scripts.Scenes.GameScene.Grid;
using App.Scripts.Scenes.GameScene.Levels;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene
{
    public class Experiments : MonoBehaviour
    {
        public TextAsset LevelDataText;
        public GameObject Prefab;

        [Inject] private IGridPositionResolver GridPositionResolver;
        
        [Button]
        public void ChangeSize()
        {
            LevelData LevelData = JsonConvert.DeserializeObject<LevelData>(LevelDataText.text);

            for (int i = 0; i < LevelData.GridSize.x * LevelData.GridSize.y; i++)
            {
                Vector2 targetPosition = GridPositionResolver.GetCurrentGridPosition();

                Transform spawned = Object.Instantiate(Prefab, targetPosition, Quaternion.identity, null).GetComponent<Transform>();
                spawned.localScale = GridPositionResolver.GetCellSize();
            }
        }
    }
}