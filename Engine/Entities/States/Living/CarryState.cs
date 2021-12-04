using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AntEngine.Colliders;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Colonies;
using AntEngine.Entities.Pheromones;
using AntEngine.Resources;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.States.Living
{
    public class CarryState : LivingState
    {
        /// <summary>
        /// Represents the total field of view in which the entity can detect obstacles.
        /// </summary>
        private const float ObstacleFieldOfView = 2F * MathF.PI / 3F;
        
        private static CarryState _instance;

        public new static CarryState Instance
        {
            get
            {
                _instance ??= new CarryState();
                return _instance;
            }
        }

        public override void OnStateUpdate(StateEntity stateEntity)
        {
            base.OnStateUpdate(stateEntity);
            
            Ant ant = (Ant) stateEntity;
            PerceptionMap perceptionMap = ant.GetPerceptionMap<HomePheromone>();
            
            float obstacleDetectRadius = ant.Transform.Scale.Length() / 2F;
            int maxDirIndex = ant.PerceptionMapPrecision;
            float positiveRotation = ant.Transform.Rotation < 0
                ? ant.Transform.Rotation + 2F * MathF.PI
                : ant.Transform.Rotation;

            int obstacleRayIndex = (int)MathF.Floor((ObstacleFieldOfView / 2) / (2 * MathF.PI) * maxDirIndex);
            
            int[] dirs = new int[3];
            dirs[0] = (int) MathF.Floor(positiveRotation / (2 * MathF.PI) * maxDirIndex);
            dirs[1] = (dirs[0] + obstacleRayIndex) % maxDirIndex;
            dirs[2] = (dirs[0] + maxDirIndex - obstacleRayIndex) % maxDirIndex;

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

            List<Colony> colonies = ant.GetSurroundingEntities<Colony>();

            foreach (Colony c in colonies)
            {
                if (ant.Home != c) continue;
                
                foreach ((Resource resource, int cost) in ant.ResourceInventory.All)
                {
                    c.Stockpile.AddResource(resource, cost);
                    ant.ResourceInventory.RemoveResource(resource, cost);
                }
                
                stateEntity.State = Next(stateEntity);

                break;
            }

            if (DateTime.Now.Subtract(ant.LastEmitTime).TotalSeconds > ant.PheromoneEmissionDelay)
            {
                ant.EmitFoodPheromone();
                ant.LastEmitTime = DateTime.Now;
            }
        }

        public new IState Next(StateEntity stateEntity)
        {
            return SearchState.Instance;
        }
    }
}