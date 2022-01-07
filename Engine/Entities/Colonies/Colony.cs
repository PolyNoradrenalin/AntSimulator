using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using AntEngine.Entities.States.Living;
using AntEngine.Resources;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Colonies
{
    /// <summary>
    ///     Entity that can spawn other entities with resources.
    /// </summary>
    public class Colony : LivingEntity
    {
        public delegate IColonyMember ColonySpawnMethod(string name, Transform transform, World world, Colony colony);

        private const string ColonyDefaultName = "Colony";

        private readonly List<IColonyMember> _population;

        private int _lastUpdateTick = 0;

        public Colony(World world, ColonySpawnMethod spawnMethod) : this(ColonyDefaultName, new Transform(), world,
            spawnMethod)
        {
        }

        public Colony(string name, Transform transform, World world, ColonySpawnMethod spawnMethod) : base(name,
            transform, world, new LivingState())
        {
            _population = new List<IColonyMember>();
            Stockpile = new ResourceInventory();
            SpawnCost = new ResourceInventory();

            SpawnMethod = spawnMethod;
        }

        /// <summary>
        ///     Current population of the colony.
        ///     This corresponds to a list of the entities that have been spawned by the colony.
        /// </summary>
        public IList<IColonyMember> Population => _population.ToImmutableList();

        /// <summary>
        ///     Current stockpile of the colony.
        ///     The resources that can be used to spawn more entities.
        /// </summary>
        public ResourceInventory Stockpile { get; }

        /// <summary>
        ///     The cost in resources of spawning one entity.
        /// </summary>
        public ResourceInventory SpawnCost { get; }

        /// <summary>
        ///     Instructions to spawn an entity of the colony.
        /// </summary>
        public ColonySpawnMethod SpawnMethod { private get; set; }

        public int SpawnDelay { get; set; } = 16;

        public int SpawnBurst { get; set; } = 6;

        /// <summary>
        ///     Where an entity should spawn.
        /// </summary>
        protected virtual Vector2 SpawnPosition
        {
            get
            {
                float angle = (float) new Random().NextDouble() * 2F * MathF.PI;
                float x = MathF.Cos(angle);
                float y = MathF.Sin(angle);

                return Transform.Position + new Vector2(x, y) * SpawnRadius * Transform.Scale;
            }
        }

        /// <summary>
        ///     Distance of spawning from the colony center.
        /// </summary>
        private float SpawnRadius { get; } = 1F;

        public override void Update()
        {
            if (_lastUpdateTick > SpawnDelay)
            {
                Spawn(SpawnBurst);
                _lastUpdateTick = 0;
            }
            else
            {
                _lastUpdateTick++;
            }
        }

        /// <summary>
        ///     Spawn entities stopping when at count or when the stockpile no longer has enough resources.
        /// </summary>
        /// <param name="count">Number of entities to spawn</param>
        public virtual void Spawn(int count = 1)
        {
            for (int i = 0; i < count && HasEnoughResources(); i++)
            {
                ConsumeResources();

                Vector2 position = SpawnPosition;
                Vector2 direction = Vector2.Normalize(position - Transform.Position);
                float angle = MathF.Atan2(direction.Y, Vector2.Dot(direction, Vector2.UnitX));
                
                IColonyMember pop = SpawnMethod("", new Transform(position, angle, Vector2.One * 10F), World, this);
                // TODO : REMOVE POP MEMBER
                _population.Add(pop);
                pop.Home = this;
            }
        }

        /// <summary>
        ///     Checks if the colony has enough resources in its stockpile to spawn an entity.
        /// </summary>
        /// <returns>True if it can, false otherwise</returns>
        protected bool HasEnoughResources()
        {
            foreach ((Resource resource, int cost) in SpawnCost.All)
            {
                if (Stockpile.All.ContainsKey(resource))
                {
                    if (Stockpile.All[resource] < cost) return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Removes from the stockpile _spawnCost resources.
        /// </summary>
        /// <returns>True if success, false otherwise</returns>
        protected void ConsumeResources()
        {
            if (!HasEnoughResources()) return;

            foreach ((Resource resource, int cost) in SpawnCost.All)
            {
                Stockpile.RemoveResource(resource, cost);
            }
        }
    }
}