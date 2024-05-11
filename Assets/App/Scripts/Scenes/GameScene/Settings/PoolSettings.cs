using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Pool Settings", fileName = "PoolSettings")]
    public class PoolSettings : SerializedScriptableObject
    {
        public MonoBehaviour View;
        public int InitialSize;
        public string ParentName;
    }
}