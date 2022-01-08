using System;
using System.Numerics;

namespace AntEngine.Utils.Maths
{
    public static class Vector2Extensions
    {
        public static float Angle(this Vector2 u, Vector2 v)
        {
            return MathF.Atan2(u.Y * v.X - u.X * v.X, u.X * v.X + u.Y * v.Y);
        }
    }
}