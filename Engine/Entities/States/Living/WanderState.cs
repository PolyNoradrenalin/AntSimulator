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
    public abstract class WanderState<T> : LivingState where T : Pheromone
    {
        private const float ObstacleFieldOfView = 2F;
        private const float ObstacleRadius = 2F;
        private const float WallAvoidanceFactor = 1000000F;

        public override void OnStateUpdate(StateEntity stateEntity)
        {
            base.OnStateUpdate(stateEntity);

            Ant ant = (Ant) stateEntity;
            PerceptionMap perceptionMap = ant.GetPerceptionMap<T>();

            // Detects obstacles in 3 directions in front of the ant.
            // The ant will be attracted by the opposite direction to avoid the obstacles.
            int maxDirIndex = perceptionMap.Weights.Count;
            float positiveRotation = ant.Transform.Rotation < 0
                ? ant.Transform.Rotation + 2F * MathF.PI
                : ant.Transform.Rotation;

            int obstacleRayIndex = (int) MathF.Min(maxDirIndex - 1 ,MathF.Round(ObstacleFieldOfView / 2 / (2 * MathF.PI) * maxDirIndex));
            
            int[] dirs = new int[3];
            dirs[0] = (int) MathF.Floor(positiveRotation / (2 * MathF.PI) * maxDirIndex);
            dirs[1] = (dirs[0] + obstacleRayIndex) % maxDirIndex;
            dirs[2] = (dirs[0] + maxDirIndex - obstacleRayIndex) % maxDirIndex;

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

            ant.Move(ant.MovementStrategy.Move(perceptionMap));
        }

        public override IState Next(StateEntity stateEntity)
        {
            return CarryState.Instance;
        }
    }
}