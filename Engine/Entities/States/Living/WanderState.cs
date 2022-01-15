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
        private const int ObstacleRayIndex = 4;
        private const float WallAvoidanceFactor = 10000F;

        public override void OnStateUpdate(StateEntity stateEntity)
        {
            base.OnStateUpdate(stateEntity);
            
            Ant ant = (Ant) stateEntity;
            PerceptionMap perceptionMap = ant.GetPerceptionMap<T>();

            // Detects obstacles in 3 directions in front of the ant.
            // The ant will be attracted by the opposite direction to avoid the obstacles.
            float obstacleDetectRadius = ant.Transform.Scale.Length() / 2F;
            int maxDirIndex = perceptionMap.Weights.Count;
            float positiveRotation = ant.Transform.Rotation < 0
                ? ant.Transform.Rotation + 2F * MathF.PI
                : ant.Transform.Rotation;

            int[] dirs = new int[3];
            dirs[0] = (int) MathF.Floor(positiveRotation / (2 * MathF.PI) * maxDirIndex);
            dirs[1] = (dirs[0] + ObstacleRayIndex) % maxDirIndex;
            dirs[2] = (dirs[0] + maxDirIndex - ObstacleRayIndex) % maxDirIndex;

            foreach (int i in dirs)
            {
                Vector2 dir = perceptionMap.Weights.Keys.ElementAt(i);
                Vector2 opposite = perceptionMap.Weights.Keys.ElementAt((i + maxDirIndex / 2) % maxDirIndex);

                HashSet<Collider> collisions = new(
                    stateEntity.World.CircleCast(
                        stateEntity.Transform.Position + dir * obstacleDetectRadius,
                        obstacleDetectRadius, true));

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