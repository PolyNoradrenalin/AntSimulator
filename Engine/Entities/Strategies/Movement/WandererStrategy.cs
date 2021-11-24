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
        private const float RandomAngleRange = 0.1F;
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
            float randomAngle = (float)new Random().NextDouble() * RandomAngleRange;
            float centeredAngle = randomAngle - RandomAngleRange / 2;
            
            Vector2 randomDir = new(MathF.Cos(centeredAngle), MathF.Sin(centeredAngle));
            Vector2 targetDir = map.Mean;

            if (targetDir.X < -0.5f)
            {
                _dir = targetDir;
            }
            else
            {
                if (_dir.X < 0)
                {
                    _dir = (1 - _random) * targetDir + _random * randomDir;
                }
                else
                {
                    _dir = _oldDirFactor * _dir + (1 - _oldDirFactor) * ((1 - _random) * targetDir + _random * randomDir);
                }
            }
            return _dir;
        }
    }
}