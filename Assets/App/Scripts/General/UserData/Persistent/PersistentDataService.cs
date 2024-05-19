using App.Scripts.External.UserData;

namespace App.Scripts.General.UserData.Persistent
{
    public class PersistentDataService : IPersistentDataService 
    {
        private readonly IUserDataContainer _userDataContainer;

        public PersistentDataService(IUserDataContainer userDataContainer)
        {
            _userDataContainer = userDataContainer;
        }
        
        public void SetLastVisit(long time)
        {
            var data = (PersistentData)_userDataContainer.GetData<PersistentData>();

            data.LastVisit = time;
            _userDataContainer.SaveData<PersistentData>();
        }

        public long GetLastVisit()
        {
            return ((PersistentData)_userDataContainer.GetData<PersistentData>()).LastVisit;
        }
    }
}