using App.Scripts.External.UserData;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace App.Scripts.General
{
    public class Exp : MonoBehaviour
    {
        private IUserDataContainer _userDataContainer;

        [Inject]
        private void Construct()
        {
            
        }
        
        [Button]
        public void Load()
        {
        }
    }
}