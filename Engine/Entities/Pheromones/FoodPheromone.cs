using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Pheromones
{
    /// <summary>
    ///     Pheromone released when an ant has found food and is heading home.
    /// </summary>
    public class FoodPheromone : Pheromone
    {
        public FoodPheromone(World world) : this(DefaultPheromoneName, new Transform(), world, DefaultMaxTimeSpan)
        {
        }

        public FoodPheromone(World world, int maxTimeSpan) : this(DefaultPheromoneName, new Transform(), world,
            maxTimeSpan)
        {
        }

        public FoodPheromone(string name, Transform transform, World world, int maxTimeSpan) : base(name, transform,
            world, maxTimeSpan)
        {
        }
    }
}