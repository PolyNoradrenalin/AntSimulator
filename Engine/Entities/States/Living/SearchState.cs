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
    ///     State of a Living Entity that searches for pheromones.
    /// </summary>
    public class SearchState : WanderState<FoodPheromone>
    {
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

            HashSet<ResourceEntity> list = ant.GetSurroundingEntities<ResourceEntity>();

            if (list.Count > 0)
            {
                ResourceEntity closest = list.Aggregate((
                    r1, r2) => r1.Transform.GetDistance(ant.Transform) < r2.Transform.GetDistance(ant.Transform)
                    ? r1
                    : r2);
                
                if (closest != null)
                {
                    if (ant.PickUp(closest))
                    {
                        stateEntity.State = Next(stateEntity);
                    }
                    else
                    {
                        stateEntity.State = new TargetState(this, closest);
                    }
                }
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