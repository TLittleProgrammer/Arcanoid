namespace App.Scripts.General.UserData.Persistent
{
    public interface IPersistentDataService
    {
        void SetLastVisit(long time);
        long GetLastVisit();
    }
}