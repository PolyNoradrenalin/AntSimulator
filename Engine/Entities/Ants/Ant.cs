using System;
using System.Collections.Generic;
using System.Numerics;
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
        public ResourceInventory ResourceInventory { get; protected set; }

        /// <summary>
        /// The distance in which the ant can perceive another entity.
        /// </summary>
        public float PerceptionDistance { get; protected set; } = 5;

        /// <summary>
        /// Precision that will determine the size of the weights list.
        /// </summary>
        public int PerceptionMapPrecision { get; } = 24;

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
        public PerceptionMap GetPerceptionMap<T>() where T : Pheromone
        {
            List<float> weights = new(new float[PerceptionMapPrecision]);

            foreach (Entity e in World.Entities)
            {
                if (e is not T) continue;
                if (!(e.Transform.GetDistance(Transform) <= PerceptionDistance)) continue;
                float rotVal = MathF.Atan2(e.Transform.Position.X - Transform.Position.X,
                    e.Transform.Position.Y - Transform.Position.Y);

                int weightListIndex;

                if (rotVal >= 0)
                {
                    weightListIndex = (int) MathF.Floor(rotVal / (2 * MathF.PI / PerceptionMapPrecision));
                }
                else
                {
                    weightListIndex =
                        (int) MathF.Floor((rotVal + 2 * MathF.PI) / (2 * MathF.PI / PerceptionMapPrecision));
                }

                float weightSum = weights[weightListIndex];
                weightSum += GetWeightFactorFromDistance(e.Transform.GetDistance(Transform));
                weightSum += GetWeightFactorFromRotation(rotVal);
                weights[weightListIndex] = weightSum;
            }

            return new PerceptionMap(weights);
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