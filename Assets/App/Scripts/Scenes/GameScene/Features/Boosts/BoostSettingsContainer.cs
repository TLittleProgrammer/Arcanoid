using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts
{
    [CreateAssetMenu(menuName = "Configs/BoostSettingsContainer", fileName = "BoostSettingsContainer")]
    public class BoostSettingsContainer : SerializedScriptableObject
    {
        public List<BoostSettingsData> BoostSettingsDatas;
    }

    public class BoostSettingsData
    {
        public string Key;
        public float Duration = 10f;
        public List<string> KeysThatCanBlockThisBoost;
        public IConcreteBoostActivator ConcreteBoostActivator;
        public IBoostDataProvider BoostDataProvider;
    }

    [CreateAssetMenu(menuName = "Configs/AvailableBoostList", fileName = "AvailableBoostList")]
    public class AvailableBoostList : ScriptableObject
    {
        public List<string> AvailableBoosts;
    }
}