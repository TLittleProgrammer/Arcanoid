using GameScene.Levels.AssetManagement;
using GameScene.Levels.Entities;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Editor.LevelEditor
{
    public class LevelGeometry : OdinEditorWindow
    {
        [TableMatrix(HorizontalTitle = "Level Grid", DrawElementMethod = "DrawElement", ResizableColumns = false, RowHeight = 32)]
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
        
        private EntityProvider DrawElement(Rect rect, EntityProvider value)
        {
            if (value is not null)
            {
                return (EntityProvider)SirenixEditorFields.UnityPreviewObjectField(
                    rect: rect,
                    value: value,
                    texture: value.EntityStages[0].Sprite.texture,
                    type: typeof(EntityProvider)
                );
            }

            return (EntityProvider)SirenixEditorFields.UnityPreviewObjectField(
                    rect: rect,
                    value: default,
                    texture: default,
                    type: typeof(EntityProvider)
                );
        } 
    }
}