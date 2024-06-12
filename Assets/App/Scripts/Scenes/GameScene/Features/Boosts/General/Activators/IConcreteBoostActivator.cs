namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public interface IConcreteBoostActivator
    {
        bool IsTimeableBoost { get; }
        void Activate(BoostTypeId boostTypeId);
        void Deactivate();
    }
}