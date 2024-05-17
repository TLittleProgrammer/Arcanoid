using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Box colliders settings", fileName = "BoxCollidersAroundScreenSettings")]
    public class BoxCollidersAroundScreenSettings : ScriptableObject
    {
        public List<BoxColliderData> BoxColliderDatas;
    }

    [Serializable]
    public class BoxColliderData
    {
        [Range(0f, 1f)]
        public float HorizontalSize;
        [Range(0f, 1f)]
        public float VerticalSize;
        [Range(-1f, 2f)]
        public float HorizontalCenter;
        [Range(-1f, 2f)]
        public float VerticalCenter;
    }
}