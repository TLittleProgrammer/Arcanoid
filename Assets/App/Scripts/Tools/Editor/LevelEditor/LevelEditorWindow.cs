using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace App.Scripts.Tools.Editor.LevelEditor
{
    public class LevelEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Level Editor")]
        private static void OpenWindow()
        {
            GetWindow<LevelEditorWindow>().Show();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;

            tree.Add("Level Geometry", CreateInstance<LevelGeometry>());
            return tree;
        }
    }
}