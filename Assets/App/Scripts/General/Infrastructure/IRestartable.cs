namespace App.Scripts.General.Infrastructure
{
    public interface IRestartable
    {
        void Restart();
    }

    public interface INewLevelRestartable : IRestartable
    {
        
    }

    public interface ICurrentLevelRestartable : IRestartable
    {
        
    }

    public interface IGeneralRestartable : IRestartable
    {
        
    }
}