using System.Numerics;
using AntEngine.Entities.Colonies;
using AntEngine.Entities.States;
using AntEngine.Entities.States.Living;
using AntEngine.Entities.Strategies.Movement;
using AntEngine.Maths;
using AntEngine.Resources;

namespace AntEngine.Entities.Ants
{
    /// <summary>
    /// Entity representing an Ant.
    /// </summary>
    public class Ant : LivingEntity, IColonyMember
    {
        private float _speed;

        public Ant(World world) : this("Ant", new Transform(), world)
        {
        }

        public Ant(string name, Transform transform, World world) : this(name, transform, world, new IdleState())
        {
        }

        public Ant(string name, Transform transform, World world, IState initialState) : base(name, transform, world,
            initialState)
        {
        }

        /// <summary>
        /// Current speed of the ant.
        /// </summary>
        public float Speed
        {
            get => _speed;
            protected set => _speed = value > MaxSpeed ? MaxSpeed : value;
        }

        /// <summary>
        /// Maximum speed of the ant.
        /// </summary>
        public float MaxSpeed { get; protected set; }

        /// <summary>
        /// Directional velocity of the ant.
        /// </summary>
        public Vector2 Velocity { get; protected set; }

        /// <summary>
        /// The ant's current movement strategy.
        /// </summary>
        public IMovementStrategy MovementStrategy { get; protected set; }
        
        /// <summary>
        /// Represents the ant's inventory.
        /// </summary>
        public ResourceDeposit ResourceInventory { get; protected set; }

        /// <summary>
        /// The distance in which the ant can perceive another entity.
        /// </summary>
        public float PerceptionDistance { get; protected set; }
        
        /// <summary>
        /// Applies movement to ant's coordinates.
        /// </summary>
        /// <param name="dir"></param>
        public void Move(Vector2 dir)
        {
            Transform.Position += dir;
        }

        /// <summary>
        /// Creates the ant's perception map.
        /// A perception map is used to represent which directions are attractive for the ant.
        /// </summary>
        /// <returns>Perception Map</returns>
        public PerceptionMap GetPerceptionMap()
        {
            // TODO: Implement, Pheromones have to be added first.
            return new PerceptionMap(new []{0,0});
        }

        public Colony Home { get; set; }
    }
}