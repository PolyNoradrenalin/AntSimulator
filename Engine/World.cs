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
        public const int WorldDivision = 64;
        
        private readonly IList<Entity> _entities;
        private IList<Entity> _entitiesAddedBuffer;
        private IList<Entity> _entitiesRemovedBuffer;
        
        public World(Vector2 size)
        {
            Size = size;
            Entities = new List<Entity>();
            Colliders = new List<Collider>();

            _entitiesAddedBuffer = new List<Entity>();
            _entitiesRemovedBuffer = new List<Entity>();
            
            Collider = new WorldCollider(new Transform(), size, WorldDivision);
            Colliders.Add(Collider);
        }

        public IList<Collider> Colliders { get; }

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
        public IEnumerable<Entity> Entities
        {
            get => _entities;
            private init => _entities = value as List<Entity>;
        }

        public int EntityCount => _entities.Count;

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
            
            ApplyAddEntity();
            ApplyRemoveEntity();
        }

        /// <summary>
        /// Registers an entity in the world.
        /// </summary>
        public void AddEntity(Entity entity)
        {
            _entitiesAddedBuffer.Add(entity);
        }

        /// <summary>
        /// Removes an entity from the world.
        /// </summary>
        public void RemoveEntity(Entity entity)
        {
            _entitiesRemovedBuffer.Add(entity);
        }

        /// <summary>
        /// Returns all colliders in a range of a specific position.
        /// </summary>
        /// <param name="position">Origin of the detection range</param>
        /// <param name="radius">Radius of the range</param>
        /// <returns>List of the colliders</returns>
        public IList<Collider> CircleCast(Vector2 position, float radius)
        {
            CircleCollider cast = new(new Transform());
            return Colliders.Where(collider => collider.CheckCollision(cast)).ToList();
        }

        private void ApplyAddEntity()
        {
            foreach (Entity entity in _entitiesAddedBuffer)
            {
                if (!Entities.Contains(entity))
                {
                    _entities.Add(entity);
                    if (entity.Collider != null) Colliders.Add(entity.Collider);
                    EntityAdded?.Invoke(entity);
                }
            }
            
            _entitiesRemovedBuffer.Clear();
        }

        private void ApplyRemoveEntity()
        {
            foreach (Entity entity in _entitiesRemovedBuffer)
            {
                bool removed = _entities.Remove(entity);
                if (removed)
                {
                    if (entity.Collider == null) Colliders.Remove(entity.Collider);
                    EntityRemoved?.Invoke(entity);
                }
            }
            
            _entitiesRemovedBuffer.Clear();
        }
    }
}