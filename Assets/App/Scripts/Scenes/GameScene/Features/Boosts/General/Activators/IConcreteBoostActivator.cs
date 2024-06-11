namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public interface IConcreteBoostActivator
    {
        void Activate(BoostTypeId boostTypeId);
        void Deactivate();
    }
}