using App.Scripts.Scenes.GameScene.Grid;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Levels.Load
{
    public sealed class LevelLoader : ILevelLoader
    {
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly GameObject _prefab;
        private LevelData _levelData;

        public LevelLoader(IGridPositionResolver gridPositionResolver, GameObject prefab)
        {
            _gridPositionResolver = gridPositionResolver;
            _prefab = prefab;
        }

        public void LoadLevel(LevelData levelData)
        {
            int blocksCounter = levelData.GridSize.x * levelData.GridSize.y;

            for (int i = 0; i < blocksCounter; i++)
            {
                Vector2 targetPosition = _gridPositionResolver.GetCurrentGridPosition();

                Transform spawned = Object.Instantiate(_prefab, targetPosition, Quaternion.identity, null).GetComponent<Transform>();
                spawned.localScale = _gridPositionResolver.GetCellSize();
            }
        }
    }
}