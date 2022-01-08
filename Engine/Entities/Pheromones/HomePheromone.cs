using AntEngine.Entities.Colonies;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Pheromones
{
    /// <summary>
    ///     Pheromone released while an Ant is searching for food.
    /// </summary>
    public class HomePheromone : Pheromone
    {
        public HomePheromone(World world, Colony colony) : this(DefaultPheromoneName, new Transform(), world, colony, DefaultMaxTimeSpan)
        {
        }

        public HomePheromone(World world, Colony colony, int maxTimeSpan) : this(DefaultPheromoneName, new Transform(), world, colony,
            maxTimeSpan)
        {
        }

        public HomePheromone(string name, Transform transform, World world, Colony colony, int maxTimeSpan) : base(name, transform,
            world, colony, maxTimeSpan)
        {
        }
    }
}