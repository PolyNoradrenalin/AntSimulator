using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AntEngine.Colliders;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Pheromones;
using AntEngine.Entities.Strategies.Movement;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.States.Living
{
    /// <summary>
    /// State of a Living Entity that searches for pheromones.
    /// </summary>
    public class SearchState : LivingState
    {
        private const int ObstacleRayIndex = 4;
        
        private static SearchState _instance;

        public new static SearchState Instance
        {
            get
            {
                _instance ??= new SearchState();
                return _instance;
            }
        }

        public override void OnStateUpdate(StateEntity stateEntity)
        {
            base.OnStateUpdate(stateEntity);

            Ant ant = (Ant) stateEntity;
            PerceptionMap perceptionMap = ant.GetPerceptionMap<FoodPheromone>();
            
            float obstacleDetectRadius = ant.Transform.Scale.Length() / 2F;
            int maxDirIndex = ant.PerceptionMapPrecision;
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

                IList<Collider> collisions = new List<Collider>(
                    stateEntity.World.CircleCast(
                    stateEntity.Transform.Position + dir * obstacleDetectRadius,
                    obstacleDetectRadius));
                
                collisions = collisions.Where(collider => collider is not CircleCollider).ToList();
                if (collisions.Count > 0)
                {
                    perceptionMap.Weights[opposite] += 1/3F;
                }
            }

            ant.Move(ant.MovementStrategy.Move(perceptionMap));

            List<ResourceEntity> list = ant.GetSurroundingEntities<ResourceEntity>();

            foreach (ResourceEntity resourceEntity in list)
            {
                if (ant.PickUp(resourceEntity))
                {
                    stateEntity.State = Next(stateEntity);
                }
                else
                {
                    stateEntity.State = new TargetState(this, resourceEntity);
                }
                
                break;
            }

            if (ant.LastEmitTime > ant.PheromoneEmissionDelay)
            {
                ant.EmitHomePheromone();
                ant.LastEmitTime = 0;
            }
            else
            {
                ant.LastEmitTime++;
            }
        }

        public override IState Next(StateEntity stateEntity)
        {
            return CarryState.Instance;
        }
    }
}