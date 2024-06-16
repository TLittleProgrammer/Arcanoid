using UnityEngine;

namespace App.Scripts.External.Components
{
    public interface IRectTransformable : IComponent
    {
        RectTransform RectTransform { get; }
    }

    public static class RectTransformExtensions
    {
        public static bool RectContainsPosition(this RectTransform rectTransform, Vector2 worldPoint)
        {
            Vector3[] worldCorners = new Vector3[4];

            rectTransform.GetWorldCorners(worldCorners);

            Vector3 bottomLeftCorner  = worldCorners[0];
            Vector3 topLeftCorner     = worldCorners[1];
            Vector3 bottomRightCorner = worldCorners[3];

            return CheckXPosition(worldPoint, bottomLeftCorner, bottomRightCorner) && CheckYPosition(worldPoint, bottomLeftCorner, topLeftCorner);
        }

        private static bool CheckYPosition(Vector2 worldPoint, Vector3 bottomLeftCorner, Vector3 topLeftCorner)
        {
            return worldPoint.y >= bottomLeftCorner.y && worldPoint.y <= topLeftCorner.y;
        }

        private static bool CheckXPosition(Vector2 worldPoint, Vector3 bottomLeftCorner, Vector3 bottomRightCorner)
        {
            return worldPoint.x >= bottomLeftCorner.x && worldPoint.x <= bottomRightCorner.x;
        }
    }
}