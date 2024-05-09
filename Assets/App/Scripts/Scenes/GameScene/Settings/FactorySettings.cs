using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Factory Settings", fileName = "FactorySettings")]
    public class FactorySettings : SerializedScriptableObject
    {
        public MonoBehaviour EntityView;
        public int InitialSize;
        public string ParentName;
    }
}