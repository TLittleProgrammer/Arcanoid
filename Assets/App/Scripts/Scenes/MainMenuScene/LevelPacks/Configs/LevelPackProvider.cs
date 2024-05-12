using System.Collections.Generic;
using App.Scripts.General.Levels;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs
{
    [CreateAssetMenu(menuName = "Configs/LevelPack/Level Pack Provider", fileName = "LevelPackProvider")]
    public class LevelPackProvider : ScriptableObject
    {
        public List<LevelPack> LevelPacks;
    }
}