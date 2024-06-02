using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.General.Providers
{
    [CreateAssetMenu(menuName = "Configs/SpriteProvider", fileName = "SpriteProvider")]
    public class SpriteProvider : SerializedScriptableObject
    {
        public Dictionary<string, Sprite> Sprites;
    }
}