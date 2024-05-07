using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Editor.LevelEditor
{
    public class LevelEditorWindow : OdinEditorWindow
    {
        [TableMatrix(HorizontalTitle = "Level Grid", DrawElementMethod = "DrawColoredEnumElement", ResizableColumns = false, RowHeight = 16)]
        public bool[,] Grid;

        [OnValueChanged("CreateGrid")]
        public int2 GridSize;
        
        [MenuItem("Tools/Level Editor")]
        private static void OpenWindow()
        {
            GetWindow<LevelEditorWindow>().Show();
        }

        private static bool DrawColoredEnumElement(Rect rect, bool value)
        {
            if (Event.current.type is EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }
            
            EditorGUI.DrawRect(rect.Padding(1), value ? Color.green : Color.black);

            return value;
        }

        [OnInspectorInit]
        private void CreateGrid()
        {
            Grid = new bool[GridSize.x, GridSize.y];
        }
    }
}