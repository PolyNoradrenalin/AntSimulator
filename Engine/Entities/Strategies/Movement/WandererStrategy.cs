using System;
using System.Numerics;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Strategies.Movement
{
    /// <summary>
    ///     This movement strategy tries to go to the mean direction of the perception map with a random factor to explore
    ///     the world.
    /// </summary>
    public class WandererStrategy : IMovementStrategy
    {
        private const float RandomAngleRange = MathF.PI / 2F;
        private const float DefaultOldDirFactor = 0.1F;

        private readonly float _oldDirFactor;
        private readonly Random _random;
        private readonly float _randomFactor;
        private Vector2 _dir;

        /// <summary>
        ///     Creates a wanderer strategy with a random factor between 0 (inclusive) and 1 (inclusive).
        ///     0 means that the direction will only be defined by the perception map and 1 only by the random direction.
        /// </summary>
        /// <param name="randomFactor"></param>
        /// <param name="oldDirFactor"></param>
        public WandererStrategy(float randomFactor, float oldDirFactor = DefaultOldDirFactor) : this(randomFactor,
            Vector2.Zero, oldDirFactor)
        {
        }

        /// <summary>
        ///     Creates a wanderer strategy with a random factor between 0 (inclusive) and 1 (inclusive).
        ///     0 means that the direction will only be defined by the perception map and 1 only by the random direction.
        /// </summary>
        /// <param name="randomFactor"></param>
        /// <param name="startDir"></param>
        /// <param name="oldDirFactor"></param>
        public WandererStrategy(float randomFactor, Vector2 startDir, float oldDirFactor = DefaultOldDirFactor)
        {
            _randomFactor = randomFactor;
            _oldDirFactor = oldDirFactor;
            _dir = startDir;
            _random = new Random();
        }

        public Vector2 Move(PerceptionMap map)
        {
            float oldDirAngle = Vector2Utils.AngleBetween(Vector2.UnitX, _dir);

            float randomAngle = (float) _random.NextDouble() * RandomAngleRange;
            float centeredAngle = randomAngle - RandomAngleRange / 2;
            Vector2 randomDir = new(MathF.Cos(centeredAngle + oldDirAngle), MathF.Sin(centeredAngle + oldDirAngle));

            Vector2 targetDir = map.Mean;
            Vector2 rawDir = (1 - _randomFactor) * targetDir + _randomFactor * randomDir;

            _dir = _oldDirFactor * _dir + (1 - _oldDirFactor) * rawDir;

            return _dir;
        }
    }
}