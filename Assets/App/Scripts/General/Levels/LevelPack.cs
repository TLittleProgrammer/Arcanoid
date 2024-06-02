using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.General.Levels
{
    [CreateAssetMenu(menuName = "Configs/LevelPack/Level Pack", fileName = "LevelPack")]
    public sealed class LevelPack : ScriptableObject
    {
        public string LocaleKey;
        public string GalacticIconKey;
        public string GalacticBackgroundKey;
        public int EnergyPrice = 3;
        public int EnergyAddForWin = 5;
        public List<TextAsset> Levels;
    }
}