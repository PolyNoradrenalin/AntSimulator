using AntEngine.Entities.Colonies;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Pheromones
{
    /// <summary>
    ///     Pheromone released when an ant has found food and is heading home.
    /// </summary>
    public class FoodPheromone : Pheromone
    {
        public FoodPheromone(World world, Colony colony) : this(DefaultPheromoneName, new Transform(), world, colony, DefaultMaxTimeSpan)
        {
        }

        public FoodPheromone(World world, Colony colony, int maxTimeSpan) : this(DefaultPheromoneName, new Transform(), world, colony,
            maxTimeSpan)
        {
        }

        public FoodPheromone(string name, Transform transform, World world, Colony colony, int maxTimeSpan) : base(name, transform,
            world, colony, maxTimeSpan)
        {
        }
    }
}