﻿using App.Scripts.External.Components;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Healthes.View
{
    public interface IHealthPointView
    {
        Image Image { get; }

        public class Factory : PlaceholderFactory<ITransformable, IHealthPointView>
        {
            
        }
    }
}