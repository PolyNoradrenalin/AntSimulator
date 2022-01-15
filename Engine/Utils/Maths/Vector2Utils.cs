using System;
using System.Numerics;

namespace AntEngine.Utils.Maths
{
    /// <summary>
    ///     Extension methods to class Vector2.
    ///     Used to avoid code duplication on common operations using Vector2.
    /// </summary>
    public static class Vector2Utils
    {
        /// <summary>
        ///     Determines the angle between two vectors.
        /// </summary>
        /// <param name="from">First vector</param>
        /// <param name="to">Second vector</param>
        /// <returns>Value in rad (-Pi to Pi) of the angle between both vectors.</returns>
        public static float AngleBetween(Vector2 from, Vector2 to)
        {
            return MathF.Atan2(to.Y, to.X) - MathF.Atan2(from.Y, from.X);
        }

        /// <summary>
        ///     Determines the angle between two vectors.
        /// </summary>
        /// <param name="from">First vector</param>
        /// <param name="to">Second vector</param>
        /// <returns>Value in rad (0 to 2 * Pi) of the angle between both vectors.</returns>
        public static float AngleBetweenNormalized(Vector2 from, Vector2 to)
        {
            float angle = AngleBetween(from, to);
            return angle < 0 ? angle + 2 * MathF.PI : angle;
        }
    }
}