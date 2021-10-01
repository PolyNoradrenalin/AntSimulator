using System.Collections.Generic;
using System.Numerics;
using AntEngine.Entities;

namespace AntEngine
{
    /// <summary>
    /// Represents the space where entities can be simulated.
    /// </summary>
    public class World
    {
        public World(Vector2 size)
        {
            Entities = new List<Entity>();
            Size = size;
        }
        
        /// <summary>
        /// List of the entities present on the map.
        /// </summary>
        public IList<Entity> Entities { get; private set; }
        
        /// <summary>
        /// Size of the world.
        /// </summary>
        public Vector2 Size { get; private set; }

        /// <summary>
        /// Updates all entities in the world.
        /// </summary>
        public void Update()
        {
            foreach (Entity entity in Entities)
            {
                entity.Update();
            }
        }
    }
}