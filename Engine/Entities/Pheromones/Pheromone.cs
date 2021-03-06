using AntEngine.Entities.Colonies;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Pheromones
{
    /// <summary>
    ///     Abstract class representing a pheromone entity.
    /// </summary>
    public abstract class Pheromone : Entity
    {
        protected const string DefaultPheromoneName = "Pheromone";
        protected const int DefaultMaxTimeSpan = 100;
        public Colony ColonyOrigin;

        public int Intensity;

        public Pheromone(World world, Colony colony) : this(DefaultPheromoneName, new Transform(), world, colony,
            DefaultMaxTimeSpan)
        {
        }

        public Pheromone(World world, Colony colony, int maxTimeSpan) : this(DefaultPheromoneName, new Transform(),
            world, colony, maxTimeSpan)
        {
        }

        public Pheromone(string name, Transform transform, World world, Colony colony, int maxTimeSpan) : base(name,
            transform, world)
        {
            Intensity = maxTimeSpan;
            ColonyOrigin = colony;
        }

        /// <summary>
        /// Decreases Intensity of pheromone on each tick.
        /// </summary>
        public override void Update()
        {
            Intensity--;

            if (Intensity <= 0) OnDecay();
        }

        protected virtual void OnDecay()
        {
            World.RemoveEntity(this);
        }
    }
}