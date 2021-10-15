using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using AntEngine.Entities.States;
using AntEngine.Entities.States.Living;
using AntEngine.Maths;
using AntEngine.Resources;

namespace AntEngine.Entities.Colonies
{
    /// <summary>
    /// Entity that can spawn other entities with resources.
    /// </summary>
    /// <typeparam name="E">The type of the entity to spawn.</typeparam>
    public class Colony : LivingEntity
    {
        private const string ColonyDefaultName = "Colony";

        public delegate IColonyMember ColonySpawnMethod(string name, Transform transform, World world, Colony colony);
        
        private List<IColonyMember> _population;
        private Dictionary<Resource, int> _stockpile;
        private Dictionary<Resource, int> _spawnCost;

        public Colony(World world, ColonySpawnMethod spawnMethod) : this(ColonyDefaultName, new Transform(), world, spawnMethod)
        {
        }

        public Colony(string name, Transform transform, World world, ColonySpawnMethod spawnMethod) : base(name, transform, world, new IdleState())
        {
            _population = new List<IColonyMember>();
            _stockpile = new Dictionary<Resource, int>();
            _spawnCost = new Dictionary<Resource, int>();

            SpawnMethod = spawnMethod;
        }

        /// <summary>
        /// Current population of the colony.
        /// This is basically the entities that have been spawned by the colony.
        /// </summary>
        public IList<IColonyMember> Population => _population.ToImmutableList();

        /// <summary>
        /// Current stockpile of the colony.
        /// The resources that can be used to spawn more entities.
        /// </summary>
        public IDictionary<Resource, int> Stockpile => _stockpile.ToImmutableDictionary();

        /// <summary>
        /// The cost in resources of spawning one entity.
        /// </summary>
        public IDictionary<Resource, int> SpawnCost => _spawnCost.ToImmutableDictionary();

        /// <summary>
        /// Distance of spawning from the colony center.
        /// </summary>
        private float SpawnRadius { get; set; } = 1F;

        /// <summary>
        /// Where an entity should spawn.
        /// </summary>
        protected virtual Vector2 SpawnPosition
        {
            get
            {
                float angle = (float)new Random().NextDouble();
                float x = MathF.Cos(angle);
                float y = MathF.Sin(angle);

                return Transform.Position + new Vector2(x, y) * SpawnRadius;
            }
        }

        /// <summary>
        /// Instructions to spawn an entity of the colony.
        /// </summary>
        public ColonySpawnMethod SpawnMethod { get; set; }

        /// <summary>
        /// Spawn entities stopping when at count or when stockpile has not enough resources.
        /// </summary>
        /// <param name="count">Number of entities to spawn</param>
        public virtual void Spawn(int count = 1)
        {
            for (int i = 0; i < count && HasEnoughResources(); i++)
            {
                ConsumeResources();

                IColonyMember pop = SpawnMethod("", new Transform(SpawnPosition, 0, Vector2.One), World, this);
                _population.Add(pop);
                pop.Home = this;
            }
        }

        /// <summary>
        /// Checks if the colony has enough resources in its stockpile to spawn an entity.
        /// </summary>
        /// <returns>True if it can, false otherwise</returns>
        protected bool HasEnoughResources()
        {
            foreach ((Resource resource, int cost) in _spawnCost)
            {
                if (_stockpile.ContainsKey(resource))
                {
                    if (_stockpile[resource] < cost) return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Removes from the stockpile _spawnCost resources. 
        /// </summary>
        /// <returns>True if success, false otherwise</returns>
        protected void ConsumeResources()
        {
            if (!HasEnoughResources()) return;
            
            foreach ((Resource resource, int cost) in _spawnCost)
            {
                _stockpile[resource] -= cost;
            }
        }
    }
}