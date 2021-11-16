using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using AntEngine.Colliders;
using AntEngine.Entities.Colonies;
using AntEngine.Entities.Pheromones;
using AntEngine.Entities.States;
using AntEngine.Entities.States.Living;
using AntEngine.Entities.Strategies.Movement;
using AntEngine.Utils.Maths;
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

        //TODO: Some attributes/properties are not initialised with the constructor. Example : MovementStrategy.
        
        public Ant(string name, Transform transform, World world, IState initialState) : base(name, transform, world,
            initialState)
        {
            
        }
        
        //TODO: Make these movement related properties not belong to only Ants.

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
        public Vector2 Velocity { get; protected set; } = Vector2.Zero;

        /// <summary>
        /// The ant's current movement strategy.
        /// </summary>
        public IMovementStrategy MovementStrategy { get; protected set; } = new LineStrategy();

        /// <summary>
        /// Represents the ant's inventory.
        /// </summary>
        public ResourceInventory ResourceInventory { get; protected set; } = new ResourceInventory();

        /// <summary>
        /// The distance in which the ant can perceive another entity.
        /// </summary>
        public float PerceptionDistance { get; protected set; } = 5F;

        /// <summary>
        /// Precision that will determine the size of the weights list.
        /// </summary>
        public int PerceptionMapPrecision { get; } = 24;

        public TimeSpan PheromoneTimeSpan { get; protected set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Applies movement to ant's coordinates.
        /// </summary>
        /// <param name="dir"></param>
        public void Move(Vector2 dir)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Creates the ant's perception map.
        /// A perception map is used to represent which directions are attractive for the ant.
        /// </summary>
        /// <returns>Perception Map</returns>
        public PerceptionMap GetPerceptionMap<T>() where T : Pheromone
        {
            List<float> weights = new(new float[PerceptionMapPrecision]);

            List <Entity> entities = GetSurroundingEntities<T>();
            
            foreach (Entity e in entities)
            {
                Vector2 antDirection = Transform.GetDirectorVector();
                Vector2 pheromoneDirection = e.Transform.Position - Transform.Position;
                
                float angleDifference = MathF.Acos(Vector2.Dot(antDirection,pheromoneDirection) / (antDirection.Length() * pheromoneDirection.Length()));

                int weightListIndex = (int) MathF.Floor(angleDifference / (2 * MathF.PI / PerceptionMapPrecision));

                float weightSum = weights[weightListIndex];
                weightSum += GetWeightFactorFromDistance(e.Transform.GetDistance(Transform));
                weightSum += GetWeightFactorFromRotation(angleDifference);
                weights[weightListIndex] = weightSum;
            }

            return new PerceptionMap(weights);
        }

        //TODO: Could be added to a higher level of entity. The only problem is that it depends on PerceptionDistance so maybe in LivingEntity?
        
        /// <summary>
        /// Generates a list of the entities that are in this Ant's perceptionDistance. 
        /// </summary>
        /// <returns>List of the entities in the perception range of this Ant</returns>
        public List<Entity> GetSurroundingEntities<T>() where T : Entity
        {
            List<Entity> list = new();
            
            foreach (Entity e in World.Entities)
            {
                if (e is not T) continue;
                if (!(e.Transform.GetDistance(Transform) <= PerceptionDistance)) continue;
                list.Add(e);
            }

            return list;
        }

        /// <summary>
        /// Allows for the Ant to pick up another Entity.
        /// </summary>
        /// <param name="e">Entity we want to pick up</param>
        public bool PickUp(ResourceEntity e)
        {
            if (Collider.CheckCollision(e.Collider))
            {
                ResourceInventory.AddResource(e.Type, e.Quantity);

                World.RemoveEntity(e);

                return true;
            }

            return false;
        }

        /// <summary>
        /// The Ant emits a home pheromone.
        /// </summary>
        public void EmitHomePheromone()
        {
            World.AddEntity(new HomePheromone(Name, Transform, World, PheromoneTimeSpan));
        }
        
        /// <summary>
        /// The Ant emits a food pheromone.
        /// </summary>
        public void EmitFoodPheromone()
        {
            World.AddEntity(new FoodPheromone(Name, Transform, World, PheromoneTimeSpan));
        }

        public Colony Home { get; set; }

        /// <summary>
        /// Returns the weight factor associated to the distance between an ant and another entity.
        /// </summary>
        /// <param name="distance">Distance between this ant and an entity</param>
        /// <returns>Weight value to be added to total weight</returns>
        private float GetWeightFactorFromDistance(float distance)
        {
            return MathF.Pow(distance, 2) != 0 ? 1 / MathF.Pow(distance, 2) : 0;
        }

        /// <summary>
        /// Returns the weight factor associated to the rotationDifference between an ant and another entity.
        /// </summary>
        /// <param name="rotationDelta">Difference in rotation between the entity and the ant</param>
        /// <returns>Weight value to be added to total weight</returns>
        private float GetWeightFactorFromRotation(float rotationDelta)
        {
            return MathF.Pow(rotationDelta, 2) != 0 ? 1 / MathF.Pow(rotationDelta, 2) : 0;
        }
    }
}