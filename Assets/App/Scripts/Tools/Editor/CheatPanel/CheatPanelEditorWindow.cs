using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace App.Scripts.Tools.Editor.LevelEditor
{
    public class CheatPanelEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Cheat Panel")]
        private static void OpenWindow()
        {
            GetWindow<CheatPanelEditorWindow>().Show();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;

            tree.Add("Cheat Panel", CreateInstance<CheatPanel>());
            return tree;
        }
    }
}