using AntEngine.Entities.States;
using AntEngine.Entities.States.Living;
using AntEngine.Maths;

namespace AntEngine.Entities
{
    /// <summary>
    /// An Entity that can live and die.
    /// </summary>
    public class LivingEntity : StateEntity
    {
        private int _health;
        public LivingEntity(World world) : this("Living Entity", new Transform(), world)
        {
        }

        public LivingEntity(string name, Transform transform, World world) : base(name, transform, world, new LivingState())
        {
        }

        /// <summary>
        /// Health of the entity.
        /// The entity dies if this falls down to zero.
        /// </summary>
        public int Health
        {
            get => _health;
            private set => _health = (value > MaxHealth) ? MaxHealth : value;
        }
        /// <summary>
        /// Maximum health of the entity.
        /// </summary>
        public int MaxHealth { get; private set; }

        /// <summary>
        /// Kills the entity.
        /// </summary>
        public void Kill()
        {
            OnEntityDeath();
            World.RemoveEntity(this);
        }

        /// <summary>
        /// Called when the entity just died.
        /// Note: this is called before the world removes the entity.
        /// </summary>
        protected virtual void OnEntityDeath()
        {
        }
    }
}