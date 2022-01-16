using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AntEngine.Colliders;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Pheromones;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.States.Living
{
    /// <summary>
    ///     This state is used as a base for any other state that needs to explore the world by following some type of pheromone.
    /// </summary>
    /// <typeparam name="T">Type of pheromone to follow</typeparam>
    public abstract class WanderState<T> : LivingState where T : Pheromone
    {
        /// <summary>
        ///     Total angle of the area covered by the circles of detection.
        /// </summary>
        private const float ObstacleFieldOfView = 2F;
        
        /// <summary>
        ///     Radius of each detection circle.
        /// </summary>
        private const float ObstacleRadius = 2F;
        
        /// <summary>
        ///     Number added on opposite directions of detected walls.
        /// </summary>
        private const float WallAvoidanceFactor = 1000000F;

        /// <summary>
        ///     Called when an entity just entered this state.
        /// </summary>
        /// <param name="stateEntity"></param>
        /// <exception cref="ArgumentException"></exception>
        public override void OnStateStart(StateEntity stateEntity)
        {
            base.OnStateStart(stateEntity);
            if (stateEntity is not Ant)
                throw new ArgumentException("States extending WanderState needs to be applied on an Ant entity.");
        }

        /// <summary>
        ///     Called at each update by the Ant. Detects the ant's surrounding looking up for pheromones and walls to
        ///     decide the ant's direction.
        /// </summary>
        /// <param name="stateEntity"></param>
        public override void OnStateUpdate(StateEntity stateEntity)
        {
            base.OnStateUpdate(stateEntity);

            Ant ant = (Ant) stateEntity;
            PerceptionMap perceptionMap = ant.GetPerceptionMap<T>(); // Sense the nearby pheromones.

            // Detects obstacles in 3 directions in front of the ant.
            // The ant will be attracted by the opposite direction to avoid the obstacles.
            int maxDirIndex = perceptionMap.Weights.Count;
            float positiveRotation = ant.Transform.Rotation < 0
                ? ant.Transform.Rotation + 2F * MathF.PI
                : ant.Transform.Rotation;

            int obstacleRayIndex = (int) MathF.Min(maxDirIndex - 1 ,MathF.Round(ObstacleFieldOfView / 2 / (2 * MathF.PI) * maxDirIndex));
            
            // Get the 3 directions of the perception map matching the cone of detection.
            int[] dirs = new int[3];
            dirs[0] = (int) MathF.Floor(positiveRotation / (2 * MathF.PI) * maxDirIndex); // Front
            dirs[1] = (dirs[0] + obstacleRayIndex) % maxDirIndex;                           // Front-Left
            dirs[2] = (dirs[0] + maxDirIndex - obstacleRayIndex) % maxDirIndex;             // Front-Right

            // Cast a circle on the 3 directions to check if a wall is here
            foreach (int i in dirs)
            {
                Vector2 dir = perceptionMap.Weights.Keys.ElementAt(i);
                Vector2 opposite = perceptionMap.Weights.Keys.ElementAt((i + maxDirIndex / 2) % maxDirIndex);

                HashSet<Collider> collisions = new(
                    stateEntity.World.CircleCast(
                        stateEntity.Transform.Position + dir * ant.Transform.Scale.Length(),
                        ObstacleRadius * ant.Transform.Scale.Length(), true));

                if (collisions.Count > 0) perceptionMap.Weights[opposite] += WallAvoidanceFactor;
            }

            // Gives the perception map to the movement strategy to get the direction the ant will go.
            ant.Move(ant.MovementStrategy.Move(perceptionMap));
        }

        public override IState Next(StateEntity stateEntity)
        {
            return CarryState.Instance;
        }
    }
}