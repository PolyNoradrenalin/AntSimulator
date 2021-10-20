using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using AntEngine.Colliders;
using AntEngine.Entities;
using AntEngine.Maths;

namespace AntEngine
{
    /// <summary>
    /// Represents the space where entities can be simulated.
    /// </summary>
    public class World
    {
        private readonly IList<Entity> _entities;
        private readonly IList<Collider> _colliders;
        
        public World(Vector2 size)
        {
            Entities = new List<Entity>();
            Size = size;
            _colliders = new List<Collider>();
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
                _colliders.Add(entity.Collider);
            }
        }

        /// <summary>
        /// Removes an entity from the world.
        /// </summary>
        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
            _colliders.Remove(entity.Collider);
        }

        /// <summary>
        /// Returns all colliders in a range of a specific position.
        /// </summary>
        /// <param name="position">Origin of the detection range</param>
        /// <param name="radius">Radius of the range</param>
        /// <returns>List of the colliders</returns>
        public IList<Collider> CircleCast(Vector2 position, float radius)
        {
            CircleCollider cast = new(new Transform(position, 0, Vector2.One * radius), new Transform());
            return _colliders.Where(collider => collider.checkCollision(cast)).ToList();
        }
    }
}