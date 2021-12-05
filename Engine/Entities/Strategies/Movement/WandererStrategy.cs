using System;
using System.Numerics;
using AntEngine.Utils.Maths;


namespace AntEngine.Entities.Strategies.Movement
{
    /// <summary>
    /// This movement strategy tries to go to the mean direction of the perception map with a random factor to explore
    /// the world.
    /// </summary>
    public class WandererStrategy : IMovementStrategy
    {
        private const float RandomAngleRange = MathF.PI / 2F;
        private const float DefaultOldDirFactor = 0.1F;

        private float _random;
        private float _oldDirFactor;
        private Vector2 _dir;

        /// <summary>
        /// Creates a wanderer strategy with a random factor between 0 (inclusive) and 1 (inclusive).
        /// 0 means that the direction will only be defined by the perception map and 1 only by the random direction.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="oldDirFactor"></param>
        public WandererStrategy(float random, float oldDirFactor = DefaultOldDirFactor)
        {
            _random = random;
            _oldDirFactor = oldDirFactor;
        }

        public Vector2 Move(PerceptionMap map)
        {
            float oldDirAngle = MathF.Atan2(_dir.Y, Vector2.Dot(_dir, Vector2.UnitX));
            
            float randomAngle = (float)new Random().NextDouble() * RandomAngleRange;
            float centeredAngle = randomAngle - RandomAngleRange / 2;
            Vector2 randomDir = new(MathF.Cos(centeredAngle + oldDirAngle), MathF.Sin(centeredAngle + oldDirAngle));
            
            Vector2 targetDir = map.Mean;
            Vector2 rawDir = (1 - _random) * targetDir + _random * randomDir;
            
            _dir = _oldDirFactor * _dir + (1 - _oldDirFactor) * rawDir;
            
            return _dir;
        }
    }
}