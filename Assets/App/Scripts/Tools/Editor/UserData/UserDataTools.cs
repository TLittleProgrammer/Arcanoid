using UnityEditor;
using UnityEngine;

namespace App.Scripts.Tools.Editor.UserData
{
    public class UserDataTools
    {
        [MenuItem("Tools/UserData/Open directory")]
        public static void OpenUserData()
        {
            EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
        }
    }
}