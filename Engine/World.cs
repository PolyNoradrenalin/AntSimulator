using System;
using System.Collections;
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

        private readonly List<Entity>[][] _regions;
        
        private IList<Entity> _entitiesAddedBuffer;
        private IList<Entity> _entitiesRemovedBuffer;
        
        public World(Vector2 size)
        {
            Size = size;
            Regions = new List<Entity>[WorldDivision][];

            for (int i = 0; i < WorldDivision; i++)
            {
                Regions[i] = new List<Entity>[WorldDivision];
                for (int j = 0; j < WorldDivision; j++)
                {
                    Regions[i][j] = new List<Entity>();
                }
            }
            
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
            get
            {
                List<Entity> retList = new List<Entity>();
                
                for (int i = 0; i < WorldDivision; i++)
                {
                    for (int j = 0; j < WorldDivision; j++)
                    {
                        retList.AddRange(Regions[i][j]);
                    }
                }

                return retList;
            }
        }

        public int EntityCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < WorldDivision; i++)
                {
                    for (int j = 0; j < WorldDivision; j++)
                    {
                        count += Regions[i][j].Count;
                    }
                }
                return count;
            }
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
        /// Stores entities into a region with all other entities in the same region.
        /// </summary>
        public List<Entity>[][] Regions
        {
            get => _regions;
            private init => _regions = value;
        }

        /// <summary>
        /// Updates all entities in the world.
        /// </summary>
        public void Update()
        {
            foreach (Entity entity in Entities)
            {
                entity.Update();
            }
            
            ApplyEntityBuffers();
        }

        /// <summary>
        /// Registers an entity in the world.
        /// This adds the entity in a buffer so that the entity can be added when needed.
        /// </summary>
        public void AddEntity(Entity entity)
        {
            _entitiesAddedBuffer.Add(entity);
        }

        /// <summary>
        /// Removes an entity from the world.
        /// This add the entity in a buffer so that the entity can be removed from the registry when needed.
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
        public IEnumerable<Collider> CircleCast(Vector2 position, float radius)
        {
            CircleCollider cast = new(new Transform(position, 0, Vector2.One * radius));
            return Colliders.Where(collider => collider.CheckCollision(cast));
        }

        /// <summary>
        /// Adds or removes the entities currently in the buffers.
        ///
        /// This method is called after a tick of the world.
        /// You should call it if you need to have Entities updated without Update.
        /// </summary>
        public void ApplyEntityBuffers()
        {
            ApplyAddEntity();
            ApplyRemoveEntity();
        }

        /// <summary>
        /// Calculates the region that a transform belongs to and returns it in the form of a pair.
        /// </summary>
        /// <param name="t"></param>
        /// <returns>Coordinates of the region in which the Transform belongs.</returns>
        public (uint, uint) GetRegionFromTransform(Transform t)
        {
            uint xVal = (uint) MathF.Floor(t.Position.X / WorldDivision);
            uint yVal = (uint) MathF.Floor(t.Position.Y / WorldDivision);

            return (xVal, yVal);
        }

        /// <summary>
        /// Allows us to get the list of entities that exist in a certain square of regions in the map.
        /// </summary>
        /// <param name="x">X coordinate of the center region of the square.</param>
        /// <param name="y">Y coordinate of the center region of the square.</param>
        /// <param name="radius">Radius that indicates how many regions we extend the list in each direction.</param>
        /// <typeparam name="T">Type of entity to get.</typeparam>
        /// <returns>A list of entities containing all entities belonging to the checked regions.</returns>
        public List<Entity> CheckEntitiesInRegion<T>(uint x, uint y, int radius)
        {
            List<Entity> list = new();

            for (int i = 0; i <= 2 * radius; i++)
            {
                for (int j = 0; j <= 2 * radius; j++)
                {
                    int xRegion = (int) x - radius + i;
                    int yRegion = (int) y - radius + j;

                    if (xRegion is < 0 or >= WorldDivision || yRegion is < 0 or >= WorldDivision) continue;
                    list.AddRange(Regions[xRegion][yRegion].Where(e => e is T));
                }
            }

            return list;
        }
        
        private void ApplyAddEntity()
        {
            foreach (Entity entity in _entitiesAddedBuffer)
            {
                if (!Entities.Contains(entity))
                {
                    (uint x, uint y) = GetRegionFromTransform(entity.Transform);
                    _regions[x][y].Add(entity);
                    if (entity.Collider != null) Colliders.Add(entity.Collider);
                    EntityAdded?.Invoke(entity);
                }
            }
            
            _entitiesAddedBuffer.Clear();
        }

        private void ApplyRemoveEntity()
        {
            foreach (Entity entity in _entitiesRemovedBuffer)
            {
                (uint x, uint y) = GetRegionFromTransform(entity.Transform);
                bool removed = _regions[x][y].Remove(entity);
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