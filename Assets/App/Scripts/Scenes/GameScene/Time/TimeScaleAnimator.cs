using App.Scripts.Scenes.GameScene.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace App.Scripts.Scenes.GameScene.Time
{
    public sealed class TimeScaleAnimator : ITimeScaleAnimator
    {
        private readonly ITimeProvider _timeProvider;
        private readonly StopGameSettings _stopGameSettings;

        public TimeScaleAnimator(ITimeProvider timeProvider, StopGameSettings stopGameSettings)
        {
            _timeProvider = timeProvider;
            _stopGameSettings = stopGameSettings;
        }
        
        public UniTask Animate(float scaleTo)
        {
            return DOVirtual.Float(_timeProvider.TimeScale, scaleTo, _stopGameSettings.TimeScaleChangeDuration, UpdateTimeScale).ToUniTask();
        }

        private void UpdateTimeScale(float value)
        {
            _timeProvider.TimeScale = value;
        }
    }
}