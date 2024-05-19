using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.General.Levels
{
    [CreateAssetMenu(menuName = "Configs/LevelPack/Level Pack Provider", fileName = "LevelPackProvider")]
    public class LevelPackProvider : ScriptableObject
    {
        public List<LevelPack> LevelPacks;
    }
}