using App.Scripts.External.UserData;
using App.Scripts.External.UserData.SaveLoad;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace App.Scripts.General
{
    public class Exp : MonoBehaviour
    {
        private IUserDataContainer _userDataContainer;

        [Inject]
        private void Construct(ISaveLoadService saveLoadService, IUserDataContainer userDataContainer)
        {
            _userDataContainer = userDataContainer;
        }
        
        [Button]
        public void Load()
        {
        }
    }
}