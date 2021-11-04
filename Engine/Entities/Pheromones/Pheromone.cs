using System;
using System.Timers;
using AntEngine.Utils;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Pheromones
{
    /// <summary>
    /// Abstract class representing a pheromone entity.
    /// </summary>
    public abstract class Pheromone : Entity
    {
        protected const string DefaultPheromoneName = "Pheromone";
        
        protected DecayTimer decayTimer;

        public Pheromone(World world) : this(DefaultPheromoneName, new Transform(), world, new TimeSpan())
        {
        }
        
        public Pheromone(World world, TimeSpan maxTimeSpan) : this(DefaultPheromoneName, new Transform(), world, maxTimeSpan)
        {
        }

        public Pheromone(string name, Transform transform, World world, TimeSpan maxTimeSpan) : base(name, transform, world)
        {
            decayTimer = new DecayTimer(maxTimeSpan);
            decayTimer.TimerDecayed += OnDecay;
        }

        public override void Update()
        {
            decayTimer.Update();
        }

        protected virtual void OnDecay(object sender, EventArgs e)
        {
            World.Entities.Remove(this);
        }
    }
}