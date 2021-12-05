using System;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Pheromones
{
    /// <summary>
    /// Pheromone released when an ant has found food and is heading home.
    /// </summary>
    public class FoodPheromone : Pheromone
    {
        public FoodPheromone(World world) : this(DefaultPheromoneName, new Transform(), world, new TimeSpan())
        {
        }

        public FoodPheromone(World world, TimeSpan maxTimeSpan) : this(DefaultPheromoneName, new Transform(), world,
            maxTimeSpan)
        {
        }

        public FoodPheromone(string name, Transform transform, World world, TimeSpan maxTimeSpan) : base(name,
            transform, world, maxTimeSpan)
        {
        }

        public override void Update()
        {
            base.Update();
        }
    }
}