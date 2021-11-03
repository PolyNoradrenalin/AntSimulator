using System;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Pheromones
{
    /// <summary>
    /// Pheromone released while an Ant is searching for food.
    /// </summary>
    public class HomePheromone : Pheromone
    {
        public HomePheromone(World world) : this(DefaultPheromoneName, new Transform(), world, new TimeSpan())
        {
        }
        
        public HomePheromone(World world, TimeSpan maxTimeSpan) : this(DefaultPheromoneName, new Transform(), world, maxTimeSpan)
        {
        }

        public HomePheromone(string name, Transform transform, World world, TimeSpan maxTimeSpan) : base(name, transform, world, maxTimeSpan)
        {
        }

        public override void Update()
        {
            base.Update();
        }
    }
}