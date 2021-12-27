using System;
using System.Collections.Generic;
using System.Numerics;
using AntEngine.Colliders;
using AntEngine.Entities.Colonies;
using AntEngine.Entities.Pheromones;
using AntEngine.Entities.States;
using AntEngine.Entities.States.Living;
using AntEngine.Entities.Strategies.Movement;
using AntEngine.Resources;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.Ants
{
    /// <summary>
    ///     Entity representing an Ant.
    /// </summary>
    public class Ant : LivingEntity, IColonyMember
    {
        private const float PheromoneMergeDistance = 5F;

        private const float DefaultMaxSpeed = 1F;

        /// <summary>
        ///     The number of ticks since the ant emitted a pheromone
        /// </summary>
        public int LastEmitTime;

        public Ant(World world) : this("Ant", new Transform(), world)
        {
        }

        public Ant(string name, Transform transform, World world) : this(name, transform, world, new SearchState())
        {
        }

        //TODO: Some attributes/properties are not initialised with the constructor. Example : MovementStrategy.

        public Ant(string name, Transform transform, World world, IState initialState) : base(name, transform, world,
            initialState)
        {
            Collider = new CircleCollider(Transform);
            MaxSpeed = DefaultMaxSpeed;
            Speed = MaxSpeed;
        }

        /// <summary>
        ///     The ant's current movement strategy.
        /// </summary>
        public IMovementStrategy MovementStrategy { get; protected set; } = new WandererStrategy(0.5f, 0.90f);

        /// <summary>
        ///     Represents the ant's inventory.
        /// </summary>
        public ResourceInventory ResourceInventory { get; protected set; } = new();

        /// <summary>
        ///     The distance in which the ant can perceive another entity.
        /// </summary>
        public float PerceptionDistance { get; protected set; } = 50F;

        /// <summary>
        ///     Precision that will determine the size of the weights list.
        /// </summary>
        public int PerceptionMapPrecision { get; } = 24;

        public int PheromoneTimeSpan { get; protected set; } = 6000;

        /// <summary>
        ///     Distance from which an ant can pick up or depose ressources.
        /// </summary>
        public float PickUpDistance { get; } = 5F;

        public int PickUpCapacity { get; set; } = 15;

        /// <summary>
        ///     Delay between each emission of a pheromone.
        /// </summary>
        public int PheromoneEmissionDelay { get; protected set; } = 5;

        public Colony Home { get; set; }

        /// <summary>
        ///     Creates the ant's perception map.
        ///     A perception map is used to represent which directions are attractive for the ant.
        /// </summary>
        /// <returns>Perception Map</returns>
        public PerceptionMap GetPerceptionMap<T>() where T : Pheromone
        {
            List<float> weights = new(new float[PerceptionMapPrecision]);

            List<T> entities = GetSurroundingEntities<T>();

            foreach (T e in entities)
            {
                Vector2 antDir = Transform.GetDirectorVector();
                Vector2 pheromoneDirection = e.Transform.Position - Transform.Position;

                float angle = MathF.Atan2(pheromoneDirection.Y, Vector2.Dot(pheromoneDirection, Vector2.UnitX));
                angle = angle < 0F ? angle + 2 * MathF.PI : angle;

                float angleDiff = MathF.Atan2(antDir.Y * pheromoneDirection.X - antDir.X * pheromoneDirection.X,
                    antDir.X * pheromoneDirection.X + antDir.Y * pheromoneDirection.Y);

                int weightListIndex = (int) MathF.Floor(angle / (2 * MathF.PI / PerceptionMapPrecision));

                float weightSum = weights[weightListIndex];
                weightSum += GetWeightFactorFromDistance(e.Transform.GetDistance(Transform)) *
                             GetWeightFactorFromRotation(angleDiff) * e.Intensity;
                weights[weightListIndex] = weightSum;
            }

            return new PerceptionMap(weights);
        }

        //TODO: Could be added to a higher level of entity. The only problem is that it depends on PerceptionDistance so maybe in LivingEntity?

        /// <summary>
        ///     Generates a list of the entities that are in this Ant's perceptionDistance.
        /// </summary>
        /// <returns>List of the entities in the perception range of this Ant</returns>
        public List<T> GetSurroundingEntities<T>() where T : Entity
        {
            return World.CheckEntitiesInRegion<T>(Region.X, Region.Y, PerceptionDistance)
                .FindAll(e => e.Transform.GetDistance(Transform) <= PerceptionDistance);
        }

        /// <summary>
        ///     Allows for the Ant to pick up another Entity.
        /// </summary>
        /// <param name="e">Entity we want to pick up</param>
        public bool PickUp(ResourceEntity e)
        {
            if (Vector2.Distance(Transform.Position, e.Transform.Position) <= PickUpDistance)
            {
                ResourceInventory.AddResource(e.Type, PickUpCapacity);
                e.RemoveResource(PickUpCapacity);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     The Ant emits a home pheromone.
        /// </summary>
        public void EmitHomePheromone()
        {
            Transform homeTransform = new(Transform.Position, 0, Vector2.One);

            if (!ReinforceNearestPheromone<HomePheromone>())
            {
                HomePheromone unused = new(Name, homeTransform, World, PheromoneTimeSpan);
            }
        }

        /// <summary>
        ///     The Ant emits a food pheromone.
        /// </summary>
        public void EmitFoodPheromone()
        {
            Transform foodTransform = new(Transform.Position, 0, Vector2.One);
            if (!ReinforceNearestPheromone<FoodPheromone>())
            {
                FoodPheromone unused = new(Name, foodTransform, World, PheromoneTimeSpan);
            }
        }

        /// <summary>
        ///     Increases the intensity of the nearest pheromone by PheromoneTimeSpan if it is in range of merge.
        /// </summary>
        /// <typeparam name="T">The type of pheromone to search</typeparam>
        /// <returns>true if a pheromone has been reinforced, false otherwise</returns>
        private bool ReinforceNearestPheromone<T>() where T : Pheromone
        {
            List<T> pheromones = World.CheckEntitiesInRegion<T>(Region.X, Region.Y, PheromoneMergeDistance);
            if (pheromones.Count == 0) return false;

            (T pheromone, float distance) candidate = (null, float.MaxValue);

            foreach (T p in pheromones)
            {
                float dist = p.Transform.GetDistance(Transform);

                if (!(dist < PheromoneMergeDistance) || !(dist < candidate.distance)) continue;
                candidate.pheromone = p;
                candidate.distance = dist;
            }

            if (candidate.pheromone != null) candidate.pheromone.Intensity += PheromoneTimeSpan;

            return candidate.pheromone != null;
        }

        /// <summary>
        ///     Returns the weight factor associated to the distance between an ant and another entity.
        /// </summary>
        /// <param name="distance">Distance between this ant and an entity</param>
        /// <returns>Weight value to be added to total weight</returns>
        private float GetWeightFactorFromDistance(float distance)
        {
            return 1F / (1 + MathF.Exp(PerceptionDistance / 2 - distance));
        }

        /// <summary>
        ///     Returns the weight factor associated to the rotationDifference between an ant and another entity.
        /// </summary>
        /// <param name="rotationDelta">Difference in rotation between the entity and the ant</param>
        /// <returns>Weight value to be added to total weight</returns>
        private float GetWeightFactorFromRotation(float rotationDelta)
        {
            float minValue = 0;
            float maxValue = 1;
            return MathF.Max(1 - MathF.Abs(rotationDelta) / MathF.PI * maxValue, minValue);
        }
    }
}