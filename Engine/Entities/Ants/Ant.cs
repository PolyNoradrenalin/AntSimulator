using System;
using System.Collections.Generic;
using System.Linq;
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
        private const float DefaultMaxSpeed = 1F;

        private static readonly Vector2 PheromoneScale = Vector2.One * 2;


        private readonly PerceptionMap _currentPerceptionMap;
        private readonly List<Vector2> _perceptionMapKeys;

        /// <summary>
        ///     The number of ticks since the ant emitted a pheromone
        /// </summary>
        public int LastEmitTime;

        /// <summary>
        ///     The number of ticks since the ant is searching food.
        ///     When this value is greater than SearchTimeout
        /// </summary>
        public int SearchTime;
        
        public Ant(World world) : this("Ant", new Transform(), world, 24)
        {
        }

        public Ant(string name, Transform transform, World world, int precision) : this(name, transform, world,
            new SearchState(), precision)
        {
        }


        //TODO: Some attributes/properties are not initialised with the constructor. Example : MovementStrategy.

        public Ant(string name, Transform transform, World world, IState initialState, int precision) : base(name,
            transform, world,
            initialState)
        {
            Collider = new CircleCollider(Transform);
            MaxSpeed = DefaultMaxSpeed;
            Speed = MaxSpeed;

            MovementStrategy = new WandererStrategy(0.5f, Transform.GetDirectorVector(), 0.90f);
            List<float> weights = new(new float[precision]);
            _currentPerceptionMap = new PerceptionMap(weights);
            _perceptionMapKeys = _currentPerceptionMap.Weights.Keys.ToList();
        }

        /// <summary>
        ///     The ant's current movement strategy.
        /// </summary>
        public IMovementStrategy MovementStrategy { get; set; }

        /// <summary>
        ///     Represents the ant's inventory.
        /// </summary>
        public ResourceInventory ResourceInventory { get; set; } = new();

        /// <summary>
        ///     The distance in which the ant can perceive another entity.
        /// </summary>
        public float PerceptionDistance { get; set; } = 50F;

        /// <summary>
        ///     Precision that will determine the size of the weights list.
        /// </summary>
        public int PerceptionMapPrecision { get; set; } = 12;

        public int PerceptionSaturationBase { get; set; } = 10000;

        public int FoodPheromoneTimeSpan { get; set; } = 1200;
        public int FoodMaxPheromoneTime { get; set; } = 1200;
        public int HomePheromoneTimeSpan { get; set; } = 6000;
        public int HomeMaxPheromoneTime { get; set; } = 10000;

        public int SearchTimeout { get; set; } = 6000;
        
        /// <summary>
        ///     Distance from which an ant can pick up or depose ressources.
        /// </summary>
        public float PickUpDistance { get; set; } = 5F;


        public int PickUpCapacity { get; set; } = 15;

        public float PheromoneMergeDistance { get; set; } = 5F;

        /// <summary>
        ///     Delay between each emission of a pheromone.
        /// </summary>
        public int PheromoneEmissionDelay { get; set; } = 30;

        public Colony Home { get; set; }

        /// <summary>
        ///     Creates the ant's perception map.
        ///     A perception map is used to represent which directions are attractive for the ant.
        /// </summary>
        /// <returns>Perception Map</returns>
        public PerceptionMap GetPerceptionMap<T>() where T : Pheromone
        {
            IEnumerable<T> entities = GetSurroundingEntities<T>().Where(pheromone => pheromone.ColonyOrigin == Home);

            foreach (Vector2 dir in _perceptionMapKeys) _currentPerceptionMap.Weights[dir] = 0;

            foreach (T e in entities)
            {
                Vector2 antDir = Transform.GetDirectorVector();
                Vector2 pheromoneDirection = e.Transform.Position - Transform.Position;

                float angle = Vector2Utils.AngleBetweenNormalized(Vector2.UnitX, pheromoneDirection);

                float angleDiff = Vector2Utils.AngleBetween(antDir, pheromoneDirection);

                int weightListIndex = Math.Min(PerceptionMapPrecision - 1, (int) MathF.Floor(angle / (2 * MathF.PI / PerceptionMapPrecision)));

                float weightSum = _currentPerceptionMap.Weights[_perceptionMapKeys[weightListIndex]];
                weightSum += GetWeightFactorFromDistance(e.Transform.GetDistance(Transform)) *
                             GetWeightFactorFromRotation(angleDiff) *
                             MathF.Log(e.Intensity + 1, PerceptionSaturationBase);
                _currentPerceptionMap.Weights[_perceptionMapKeys[weightListIndex]] = weightSum;
            }

            return _currentPerceptionMap;
        }

        //TODO: Could be added to a higher level of entity. The only problem is that it depends on PerceptionDistance so maybe in LivingEntity?

        /// <summary>
        ///     Generates a list of the entities that are in this Ant's perceptionDistance.
        /// </summary>
        /// <returns>List of the entities in the perception range of this Ant</returns>
        public HashSet<T> GetSurroundingEntities<T>() where T : Entity
        {
            HashSet<T> entities = World.CheckEntitiesInRegion<T>(Region.X, Region.Y, PerceptionDistance);
            HashSet<T> filteredEntities = new();

            foreach (T e in entities)
                if (e.Transform.GetDistance(Transform) <= PerceptionDistance)
                    filteredEntities.Add(e);

            return filteredEntities;
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
            Transform homeTransform = new(Transform.Position, 0, PheromoneScale);

            if (!ReinforceNearestPheromone<HomePheromone>(HomePheromoneTimeSpan, HomeMaxPheromoneTime))
            {
                HomePheromone unused = new(Name, homeTransform, World, Home, HomePheromoneTimeSpan);
            }
        }

        /// <summary>
        ///     The Ant emits a food pheromone.
        /// </summary>
        public void EmitFoodPheromone()
        {
            Transform foodTransform = new(Transform.Position, 0, PheromoneScale);
            if (!ReinforceNearestPheromone<FoodPheromone>(FoodPheromoneTimeSpan, FoodMaxPheromoneTime))
            {
                FoodPheromone unused = new(Name, foodTransform, World, Home, FoodPheromoneTimeSpan);
            }
        }

        /// <summary>
        ///     Increases the intensity of the nearest pheromone by PheromoneTimeSpan if it is in range of merge.
        /// </summary>
        /// <typeparam name="T">The type of pheromone to search</typeparam>
        /// <returns>true if a pheromone has been reinforced, false otherwise</returns>
        private bool ReinforceNearestPheromone<T>(int intensity, int maxIntensity) where T : Pheromone
        {
            HashSet<T> pheromones = World.CheckEntitiesInRegion<T>(Region.X, Region.Y, PheromoneMergeDistance);
            if (pheromones.Count == 0) return false;

            (T pheromone, float distance) candidate = (null, float.MaxValue);

            foreach (T p in pheromones)
            {
                float dist = p.Transform.GetDistance(Transform);

                if (!(dist < PheromoneMergeDistance) || !(dist < candidate.distance)) continue;
                candidate.pheromone = p;
                candidate.distance = dist;
            }

            if (candidate.pheromone != null)
                candidate.pheromone.Intensity = Math.Min(candidate.pheromone.Intensity + intensity, maxIntensity);

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
            float minValue = 0.2F;
            float maxValue = 1;
            return MathF.Max((1 - MathF.Abs(rotationDelta) / MathF.PI) * maxValue, minValue);
        }
    }
}