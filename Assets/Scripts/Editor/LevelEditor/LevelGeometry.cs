using GameScene.Levels.AssetManagement;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Editor.LevelEditor
{
    public class LevelGeometry : OdinEditorWindow
    {
        [TableMatrix(HorizontalTitle = "Level Grid", DrawElementMethod = "DrawEntityProvider", ResizableColumns = false, RowHeight = 32)]
        public EntityProvider[,] Grid;
        [OnValueChanged("CreateGrid")]
        public int2 GridSize;

        [MenuItem("Tools/Level Editor")]
        private static void OpenWindow()
        {
            GetWindow<LevelGeometry>().Show();
        }

        [OnInspectorInit]
        private void CreateGrid()
        {
            Grid = new EntityProvider[GridSize.x, GridSize.y];
        }

        private static EntityProvider DrawEntityProvider(Rect rect, EntityProvider value)
        {


            return value;
        }
    }
}