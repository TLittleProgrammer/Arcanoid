using System;
using UnityEngine;

namespace App.Scripts.General.Reactive
{
    public class ReactiveProperty<T>
    {
        public event Action<T> OnChanged;

        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;

                OnChanged?.Invoke(_value);
            }
        }
    }
}