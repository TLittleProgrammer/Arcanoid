using System;
using Newtonsoft.Json;

namespace App.Scripts.Tests.Datas
{
    [Serializable]
    public class AngleToVectorTestData
    {
        [JsonProperty("TargetAngle")]
        public float Angle;
        [JsonProperty("Answer")]
        public Vector Vector;
    }

    [Serializable]
    public class Vector : IComparable<Vector>, IEquatable<Vector>
    {
        [JsonProperty("X")]
        public float X;
        [JsonProperty("Y")]
        public float Y;

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public int CompareTo(Vector other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0) return xComparison;
            return Y.CompareTo(other.Y);
        }

        public bool Equals(Vector other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}