namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public class BoostData
    {
        public string BoostTypeId;
        public float Duration;

        public BoostData()
        {
        }

        public BoostData(string boostTypeId, float duration)
        {
            BoostTypeId = boostTypeId;
            Duration = duration;
        }
    }
}