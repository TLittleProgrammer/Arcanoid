namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public static class BoostExtensions
    {
        public static bool IsPositiveBoost(this BoostTypeId boostTypeId)
        {
            return boostTypeId is
                BoostTypeId.Autopilot or
                BoostTypeId.Fireball or
                BoostTypeId.AddHealth or
                BoostTypeId.BallAcceleration or
                BoostTypeId.CaptiveBall or
                BoostTypeId.MiniGun or
                BoostTypeId.StickyPlatform or
                BoostTypeId.PlayerShapeAddSize or
                BoostTypeId.PlayerShapeAddSpeed;
        }
    }
}