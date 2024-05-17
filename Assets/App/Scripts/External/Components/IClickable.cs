using System;
using UnityEngine.EventSystems;

namespace App.Scripts.External.Components
{
    public interface IClickable : IComponent, IPointerClickHandler
    {
        event Action Clicked;
    }
}