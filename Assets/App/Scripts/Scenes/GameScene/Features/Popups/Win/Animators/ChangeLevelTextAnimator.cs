using App.Scripts.General.Animator;
using App.Scripts.General.Levels;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Popups.Animators
{
    public class ChangeLevelTextAnimator : MonoAnimator<LevelPack, LevelPack>
    {
        [SerializeField] private TMP_Text _passedLevels;
        
        public override async UniTask Animate(LevelPack currentLevelPack, LevelPack nextLevelPack)
        {
            await DOVirtual.Float(0f, 1f, 1f, (value) =>
            {
                int currentLevel = (int)Mathf.Lerp(currentLevelPack.Levels.Count, 0, value);
                int maxLevelsCount = (int)Mathf.Lerp(currentLevelPack.Levels.Count, nextLevelPack.Levels.Count, value);

                _passedLevels.text = $"{currentLevel.ToString()}/{maxLevelsCount.ToString()}";
            });
        }

        public override UniTask UndoAnimate()
        {
            return UniTask.CompletedTask;
        }
    }
}