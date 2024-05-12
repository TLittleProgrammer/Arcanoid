namespace App.Scripts.General.AnimatableButtons
{
    public interface IAnimatableButton<TParam>
    {
        void Play(TParam param);
        void Stop();
    }
}