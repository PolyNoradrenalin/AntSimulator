using System.Data;
using AntEngine.Maths;

namespace AntEngine.Entity
{
    /// <summary>
    /// Base class for all things that can exist on a World.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Identifier of the entity.
        /// Names are not necessarily unique but it's better to distinguish entities. 
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Transform of the entity in the world.
        /// Represents the position, rotation and scale of the entity in the world.
        /// </summary>
        public Transform Transform { get; private set; }

        /// <summary>
        /// Called by the world when this entity needs to update the status of each of its components.
        /// </summary>
        public abstract void Update();
    }
}