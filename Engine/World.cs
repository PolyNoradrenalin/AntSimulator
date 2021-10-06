using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using AntEngine.Entities;

namespace AntEngine
{
    /// <summary>
    /// Represents the space where entities can be simulated.
    /// </summary>
    public class World
    {
        private readonly IList<Entity> _entities;

        public World(Vector2 size)
        {
            Entities = new List<Entity>();
            Size = size;
        }

        /// <summary>
        /// List of the entities present on the map.
        /// </summary>
        public IList<Entity> Entities
        {
            get => _entities.ToImmutableList();
            private init => _entities = value;
        }
        
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

        /// <summary>
        /// Registers an entity in the world.
        /// </summary>
        public void AddEntity(Entity entity)
        {
            if (!Entities.Contains(entity))
            {
                _entities.Add(entity);
            }
        }

        /// <summary>
        /// Removes an entity from the world.
        /// </summary>
        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }
    }
}