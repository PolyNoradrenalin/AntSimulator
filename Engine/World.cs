using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using AntEngine.Colliders;
using AntEngine.Entities;
using AntEngine.Utils.Maths;

namespace AntEngine
{
    /// <summary>
    /// Represents the space where entities can be simulated.
    /// </summary>
    public class World
    {
        public const int WorldDivision = 1024;
        
        private readonly IList<Entity> _entities;
        private readonly IList<Collider> _colliders;
        
        public World(Vector2 size)
        {
            Entities = new List<Entity>();
            Size = size;
            _colliders = new List<Collider>();

           Collider = new WorldCollider(new Transform(), new Transform(), size, WorldDivision);
            _colliders.Add(Collider);
        }

        /// <summary>
        /// Called when an entity is spawned in the world.
        /// </summary>
        public event Action<Entity> EntityAdded;
        /// <summary>
        /// Called when an entity is removed from the world.
        /// </summary>
        public event Action<Entity> EntityRemoved;
        
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
        /// Collider of the world (the walls).
        /// </summary>
        public WorldCollider Collider { get; private set; }
        
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
                EntityAdded?.Invoke(entity);
            }
        }

        /// <summary>
        /// Removes an entity from the world.
        /// </summary>
        public void RemoveEntity(Entity entity)
        {
            bool removed = _entities.Remove(entity);
            if (removed)
            {
                _colliders.Remove(entity.Collider);
                EntityRemoved?.Invoke(entity);
            }
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
            return _colliders.Where(collider => collider.CheckCollision(cast)).ToList();
        }
    }
}