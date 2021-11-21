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
        private const float ObstacleDetectRadius = 10F;
        private const int ObstacleIndexDivisor = 5;
        
        private static SearchState _instance;

        public new static SearchState Instance
        {
            get
            {
                _instance ??= new SearchState();
                return _instance;
            }
        }

        private DateTime _lastEmit;

        public override void OnStateUpdate(StateEntity stateEntity)
        {
            base.OnStateUpdate(stateEntity);
            
            Ant ant = (Ant) stateEntity;
            PerceptionMap perceptionMap = ant.GetPerceptionMap<FoodPheromone>();
            int maxDirIndex = ant.PerceptionMapPrecision;

            List<Vector2> dirs = new();
            dirs.Add(perceptionMap.Weights.Keys.ElementAt(0));
            dirs.Add(perceptionMap.Weights.Keys.ElementAt(maxDirIndex / ObstacleIndexDivisor - 1));
            dirs.Add(perceptionMap.Weights.Keys.ElementAt(maxDirIndex - maxDirIndex / ObstacleIndexDivisor - 1));

            foreach (Vector2 dir in dirs)
            {
                Vector2 globalDir = new Vector2(
                    MathF.Cos(ant.Transform.Rotation) * dir.X - MathF.Sin(ant.Transform.Rotation) * dir.Y,
                    MathF.Sin(ant.Transform.Rotation) * dir.X + MathF.Cos(ant.Transform.Rotation) * dir.Y
                );
                
                IList<Collider> collisions = new List<Collider>(
                    stateEntity.World.CircleCast(
                    stateEntity.Transform.Position + globalDir * ObstacleDetectRadius,
                    ObstacleDetectRadius));
                
                collisions = collisions.Where(collider => collider is not CircleCollider).ToList();
                if (collisions.Count > 0)
                {
                    perceptionMap.Weights[dir] -= 1F;
                }
            }

            ant.Move(ant.MovementStrategy.Move(perceptionMap));

            List<Entity> list = ant.GetSurroundingEntities<ResourceEntity>();

            foreach (Entity e in list)
            {
                if (e is ResourceEntity resourceEntity)
                {
                    ant.PickUp(resourceEntity);

                    stateEntity.State = Next(stateEntity);

                    break;
                }
            }

            if (DateTime.Now.Subtract(_lastEmit).TotalSeconds > ant.PheromoneEmissionDelay)
            {
                ant.EmitHomePheromone();
                _lastEmit = DateTime.Now;
            }
        }

        public new IState Next(StateEntity stateEntity)
        {
            return CarryState.Instance;
        }
    }
}