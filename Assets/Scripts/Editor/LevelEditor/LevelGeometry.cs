using GameScene.Levels.AssetManagement;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Editor.LevelEditor
{
    public class LevelGeometry : OdinEditorWindow
    {
        //Geometry
        [TabGroup("Editor", "Geometry", SdfIconType.Map, TextColor = "green")]
        [TableMatrix(HorizontalTitle = "Level Grid", DrawElementMethod = "DrawElement", ResizableColumns = false, RowHeight = 32)]
        public EntityProvider[,] Grid;
        
        
        //Parameters
        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        [OnValueChanged("CreateGrid")]
        public int2 GridSize;
        
        
        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        [PreviewField(75)]
        public EntityProvider EntityProvider;

        private bool _brushIsEnabled = false;

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
            if (Event.current.type == EventType.MouseDrag && rect.Contains(Event.current.mousePosition))
            {
                GUI.changed = true;
                Event.current.Use();
                
                if (_brushIsEnabled)
                {
                    value = EntityProvider;
                    return (EntityProvider)SirenixEditorFields.UnityPreviewObjectField(
                        rect: rect,
                        value: value,
                        texture: value.EntityStages[0].Sprite.texture,
                        type: typeof(EntityProvider)
                    );
                }
            }

            return (EntityProvider)SirenixEditorFields.UnityPreviewObjectField(
                rect: rect,
                value: value,
                texture: (value is null ? default : value.EntityStages[0].Sprite.texture),
                type: typeof(EntityProvider)
            );
        }

        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        [ShowIf("_brushIsEnabled")]
        [HorizontalGroup("Editor/Split", 0.5f)]
        [Button(SdfIconType.BrushFill, IconAlignment = IconAlignment.LeftOfText, Name = "Brush"), GUIColor(0.4f, 0.8f, 1)]
        private void OnBrushButtonClickedEnabled()
        {
            _brushIsEnabled = false;
        }
        
        [TabGroup("Editor", "Level Parameters", SdfIconType.Magic, TextColor = "orange")]
        [HideIf("_brushIsEnabled")]
        [HorizontalGroup("Editor/Split", 0.5f)]
        [Button(SdfIconType.BrushFill, IconAlignment = IconAlignment.LeftOfText, Name = "Brush")]
        private void OnBrushButtonClickedDisabled()
        {
            _brushIsEnabled = true;
        }
    }
}