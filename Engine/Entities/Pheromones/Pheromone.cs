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

        public int Intensity;

        public Pheromone(World world) : this(DefaultPheromoneName, new Transform(), world, DefaultMaxTimeSpan)
        {
        }

        public Pheromone(World world, int maxTimeSpan) : this(DefaultPheromoneName, new Transform(), world, maxTimeSpan)
        {
        }

        public Pheromone(string name, Transform transform, World world, int maxTimeSpan) : base(name, transform, world)
        {
            Intensity = maxTimeSpan;
        }

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