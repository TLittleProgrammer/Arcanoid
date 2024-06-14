using UnityEngine;

namespace App.Scripts.Tests.Math
{
    public static class MathService
    {
        public static Vector2 GetDirectionByAngle(float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }
    }
}