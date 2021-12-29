using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AntEngine.Colliders;
using AntEngine.Entities;
using AntEngine.Utils.Maths;

namespace AntEngine
{
    /// <summary>
    ///     Represents the space where entities can be simulated.
    /// </summary>
    public class World
    {
        public const int WorldColliderDivision = 64;
        public const int WorldRegionDivision = 256;

        private readonly IList<Entity> _entitiesAddedBuffer;
        private readonly IList<Entity> _entitiesRemovedBuffer;
        private readonly IList<Entity> _entitiesUpdatedBuffer;

        public World(Vector2 size)
        {
            Size = size;
            Regions = new List<Entity>[WorldRegionDivision][];

            for (int i = 0; i < WorldRegionDivision; i++)
            {
                Regions[i] = new List<Entity>[WorldRegionDivision];
                for (int j = 0; j < WorldRegionDivision; j++)
                {
                    Regions[i][j] = new List<Entity>();
                }
            }

            _entitiesAddedBuffer = new List<Entity>();
            _entitiesUpdatedBuffer = new List<Entity>();
            _entitiesRemovedBuffer = new List<Entity>();

            Collider = new WorldCollider(new Transform(), size, WorldColliderDivision);
        }

        /// <summary>
        ///     List of the entities present on the map.
        /// </summary>
        public IEnumerable<Entity> Entities
        {
            get
            {
                List<Entity> retList = new();

                for (int i = 0; i < WorldRegionDivision; i++)
                for (int j = 0; j < WorldRegionDivision; j++)
                {
                    retList.AddRange(Regions[i][j]);
                }

                return retList;
            }
        }

        public int EntityCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < WorldRegionDivision; i++)
                for (int j = 0; j < WorldRegionDivision; j++)
                {
                    count += Regions[i][j].Count;
                }

                return count;
            }
        }

        /// <summary>
        ///     Size of the world.
        /// </summary>
        public Vector2 Size { get; }

        /// <summary>
        ///     Collider of the world (the walls).
        /// </summary>
        public WorldCollider Collider { get; }

        /// <summary>
        ///     Stores entities into a region with all other entities in the same region.
        /// </summary>
        public List<Entity>[][] Regions { get; }

        /// <summary>
        ///     Called when an entity is spawned in the world.
        /// </summary>
        public event Action<Entity> EntityAdded;

        /// <summary>
        ///     Called when an entity is removed from the world.
        /// </summary>
        public event Action<Entity> EntityRemoved;

        /// <summary>
        ///     Updates all entities in the world.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < WorldRegionDivision; i++)
            for (int j = 0; j < WorldRegionDivision; j++)
            {
                foreach (Entity e in Regions[i][j])
                {
                    e.Update();
                }
            }

            ApplyEntityBuffers();
        }

        /// <summary>
        ///     Registers an entity in the world.
        ///     This adds the entity in a buffer so that the entity can be added when needed.
        /// </summary>
        public void AddEntity(Entity entity)
        {
            _entitiesAddedBuffer.Add(entity);
        }

        /// <summary>
        ///     Updates the region of an entity.
        /// </summary>
        /// <param name="entity">Entity to be updated</param>
        public void UpdateEntityRegion(Entity entity)
        {
            _entitiesUpdatedBuffer.Add(entity);
        }

        /// <summary>
        ///     Removes an entity from the world.
        ///     This add the entity in a buffer so that the entity can be removed from the registry when needed.
        /// </summary>
        public void RemoveEntity(Entity entity)
        {
            _entitiesRemovedBuffer.Add(entity);
        }

        /// <summary>
        ///     Adds or removes the entities currently in the buffers.
        ///     This method is called after a tick of the world.
        ///     You should call it if you need to have Entities updated without Update.
        /// </summary>
        public void ApplyEntityBuffers()
        {
            ApplyRemoveEntity();
            ApplyUpdateEntity();
            ApplyAddEntity();
        }

        /// <summary>
        ///     Calculates the region that a transform belongs to and returns it in the form of a pair.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Coordinates of the region in which the Transform belongs.</returns>
        public (int, int) GetRegionFromPosition(Vector2 position)
        {
            int xVal = (int) MathF.Floor(position.X / Size.X * WorldRegionDivision);
            int yVal = (int) MathF.Floor(position.Y / Size.Y * WorldRegionDivision);

            return (xVal, yVal);
        }

        /// <summary>
        ///     Returns all colliders in a range of a specific position.
        /// </summary>
        /// <param name="position">Origin of the detection range</param>
        /// <param name="radius">Radius of the range</param>
        /// <returns>List of the colliders</returns>
        public IEnumerable<Collider> CircleCast(Vector2 position, float radius)
        {
            (int x, int y) = GetRegionFromPosition(position);

            List<Entity> entities = CheckEntitiesInRegion<Entity>(x, y, radius);
            List<Collider> colliders = (from e in entities where e.Collider != null select e.Collider).ToList();
            CircleCollider circle = new(new Transform(position, 0, Vector2.One * radius));
            if (Collider.CheckCollision(circle)) colliders.Add(Collider);
            return colliders;
        }

        /// <summary>
        ///     Allows us to get the list of entities that exist in a certain square of regions in the map.
        /// </summary>
        /// <param name="x">X coordinate of the center region of the square.</param>
        /// <param name="y">Y coordinate of the center region of the square.</param>
        /// <param name="radius">Radius that indicates how many regions we extend the list in each direction.</param>
        /// <typeparam name="T">Type of entity to get.</typeparam>
        /// <returns>A list of entities containing all entities belonging to the checked regions.</returns>
        public List<T> CheckEntitiesInRegion<T>(int x, int y, int radius) where T : Entity
        {
            List<T> list = new();

            for (int i = 0; i <= 2 * radius; i++)
            for (int j = 0; j <= 2 * radius; j++)
            {
                int xRegion = x - radius + i;
                int yRegion = y - radius + j;

                if (xRegion is < 0 or >= WorldRegionDivision || yRegion is < 0 or >= WorldRegionDivision) continue;
                foreach (Entity e in Regions[xRegion][yRegion])
                {
                    if (e is T entity)
                        list.Add(entity);
                }
            }

            return list;
        }

        public List<T> CheckEntitiesInRegion<T>(int x, int y, float radius) where T : Entity
        {
            float minSize = MathF.Min(Size.X, Size.Y);
            return CheckEntitiesInRegion<T>(x, y, (int) (radius / minSize * WorldRegionDivision));
        }

        private void ApplyAddEntity()
        {
            foreach (Entity entity in _entitiesAddedBuffer)
            {
                (int x, int y) = GetRegionFromPosition(entity.Transform.Position);

                if (Regions[x][y].Contains(entity)) continue;

                entity.Region = (x, y);
                Regions[x][y].Add(entity);
                EntityAdded?.Invoke(entity);
            }

            _entitiesAddedBuffer.Clear();
        }

        private void ApplyUpdateEntity()
        {
            foreach (Entity entity in _entitiesUpdatedBuffer)
            {
                (int x, int y) = GetRegionFromPosition(entity.Transform.Position);

                if (entity.Region == (x, y)) continue;

                Regions[entity.Region.X][entity.Region.Y].Remove(entity);
                entity.Region = (x, y);
                Regions[entity.Region.X][entity.Region.Y].Add(entity);
            }

            _entitiesUpdatedBuffer.Clear();
        }

        private void ApplyRemoveEntity()
        {
            foreach (Entity entity in _entitiesRemovedBuffer)
            {
                (int x, int y) = GetRegionFromPosition(entity.Transform.Position);
                _entitiesUpdatedBuffer.Remove(entity);
                bool removed = Regions[x][y].Remove(entity);
                if (removed) EntityRemoved?.Invoke(entity);
            }

            _entitiesRemovedBuffer.Clear();
        }
    }
}