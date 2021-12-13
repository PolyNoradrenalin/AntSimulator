using System.Numerics;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Strategies.Movement
{
    /// <summary>
    ///     Defines how an entity should move in function of where it wants to move.
    /// </summary>
    public interface IMovementStrategy
    {
        /// <summary>
        ///     Gives the direction the entity should go to respect the strategy.
        /// </summary>
        /// <param name="map">A weighted perception map representing how much the entity wants to go in each direction.</param>
        /// <returns>The target direction.</returns>
        Vector2 Move(PerceptionMap map);
    }
}