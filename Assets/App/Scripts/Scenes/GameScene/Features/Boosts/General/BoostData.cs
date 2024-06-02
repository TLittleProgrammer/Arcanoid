﻿namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public class BoostData
    {
        public BoostTypeId BoostTypeId;
        public float Duration;

        public BoostData()
        {
        }

        public BoostData(BoostTypeId boostTypeId, float duration)
        {
            BoostTypeId = boostTypeId;
            Duration = duration;
        }
    }
}