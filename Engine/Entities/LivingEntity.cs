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
        public int Health { get; private set; }
        /// <summary>
        /// Maximum health of the entity.
        /// </summary>
        public int MaxHealth { get; private set; }
    }
}