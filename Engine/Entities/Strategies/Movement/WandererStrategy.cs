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
        private float _random;
        private Vector2 _dir;
        
        /// <summary>
        /// Creates a wanderer strategy with a random factor between 0 (inclusive) and 1 (inclusive).
        /// 0 means that the direction will only be defined by the perception map and 1 only by the random direction.
        /// </summary>
        /// <param name="random"></param>
        public WandererStrategy(float random)
        {
            _random = random;
        }

        public Vector2 Move(PerceptionMap map)
        {
            Vector2 targetDir = map.Mean;
            Vector2 randomDir = new((float)new Random().NextDouble(), (float)new Random().NextDouble() * 0.2f - 0.1f);
            _dir = (_dir + 0.5f * ((1 - _random) * targetDir + _random * randomDir)) / 2;
            return _dir;
        }
    }
}