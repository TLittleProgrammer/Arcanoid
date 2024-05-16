﻿using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.States
{
    public class GameLoopState : IState, ITickable
    {
        private readonly IEnumerable<ITickable> _tickables;
        
        private bool _stateIsEntered = false;
        
        public GameLoopState(IEnumerable<ITickable> tickables)
        {
            _tickables = tickables;
        }
        
        public void Enter()
        {
            _stateIsEntered = true;
        }

        public void Exit()
        {
            _stateIsEntered = false;
        }

        public void Tick()
        {
            foreach (ITickable tickable in _tickables)
            {
                tickable.Tick();
            }
        }
    }
}