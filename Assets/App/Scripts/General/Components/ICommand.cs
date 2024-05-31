namespace App.Scripts.General.Components
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<TParam1>
    {
        void Execute(TParam1 param1);
    }
    
    public interface ICommand<TParam1, TParam2>
    {
        void Execute(TParam1 param1, TParam2 param2);
    }
}