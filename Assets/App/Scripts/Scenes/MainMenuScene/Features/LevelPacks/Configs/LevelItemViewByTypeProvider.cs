using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenuScene.Features.LevelPacks.Configs
{
    [CreateAssetMenu(menuName = "Configs/LevelPack/Level Pack View By Type Provider", fileName = "LevelPackViewByTypeProvider")]
    public class LevelItemViewByTypeProvider : SerializedScriptableObject
    {
        public Dictionary<VisualTypeId, LevelItemViewData> Views;
    }
}