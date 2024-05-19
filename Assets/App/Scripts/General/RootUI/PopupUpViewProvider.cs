using App.Scripts.External.Components;
using UnityEngine;

namespace App.Scripts.General.RootUI
{
    public class PopupUpViewProvider : MonoBehaviour, ITransformable
    {
        public Transform Transform => transform;
    }
}