using System;
using System.Numerics;

namespace AntEngine.Utils.Maths
{
    public static class Vector2Utils
    {
        public static float AngleBetween(Vector2 from, Vector2 to)
        {
            return MathF.Atan2(to.Y, to.X) - MathF.Atan2(from.Y, from.X);
        }
        
        public static float AngleBetweenNormalized(Vector2 from, Vector2 to)
        {
            float angle = AngleBetween(from, to);
            return angle < 0 ? angle + 2 * MathF.PI : angle;
        }
    }
}