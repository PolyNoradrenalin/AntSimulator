using System.Numerics;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Strategies.Movement
{
    /// <summary>
    /// Movement strategy that go in a straight line in the mean direction of its perception map.
    /// </summary>
    public class LineStrategy : IMovementStrategy
    {
        public Vector2 Move(PerceptionMap map)
        {
            return map.Mean;
        }
    }
}