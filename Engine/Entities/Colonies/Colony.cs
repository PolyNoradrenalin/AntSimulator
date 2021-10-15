using System.Collections.Generic;
using System.Collections.Immutable;
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
    public class Colony<E> : LivingEntity where E : LivingEntity, new()
    {
        private const string ColonyDefaultName = "Colony";
        
        private List<E> _population;
        private Dictionary<Resource, int> _stockpile;
        private Dictionary<Resource, int> _spawnCost;

        public Colony(World world) : this(ColonyDefaultName, new Transform(), world)
        {
        }

        public Colony(string name, Transform transform, World world) : this(name, transform, world, new IdleState())
        {
        }

        public Colony(string name, Transform transform, World world, IState initialState) : base(name, transform, world, initialState)
        {
            _population = new List<E>();
            _stockpile = new Dictionary<Resource, int>();
            _spawnCost = new Dictionary<Resource, int>();
        }

        /// <summary>
        /// Current population of the colony.
        /// This is basically the entities that have been spawned by the colony.
        /// </summary>
        public IList<E> Population => _population.ToImmutableList();
        /// <summary>
        /// Current stockpile of the colony.
        /// The resources that can be used to spawn more entities.
        /// </summary>
        public IDictionary<Resource, int> Stockpile => _stockpile.ToImmutableDictionary();
        /// <summary>
        /// The cost in resources of spawning one entity.
        /// </summary>
        public IDictionary<Resource, int> SpawnCost => _spawnCost.ToImmutableDictionary();
    }
}