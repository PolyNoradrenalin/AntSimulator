using System.Collections.Generic;
using System.Linq;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Pheromones;

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
        
        public override void OnStateStart(StateEntity stateEntity)
        {
            base.OnStateStart(stateEntity);
            
            Ant ant = (Ant) stateEntity;
            ant.SearchTime = 0;
        }

        public override void OnStateUpdate(StateEntity stateEntity)
        {
            base.OnStateUpdate(stateEntity);

            Ant ant = (Ant) stateEntity;

            HashSet<ResourceEntity> list = ant.GetSurroundingEntities<ResourceEntity>();

            // When the Ant detects some food, we select the closest one and try to pick it up.
            if (list.Count > 0)
            {
                ResourceEntity closest = list.Aggregate((
                    r1, r2) => r1.Transform.GetDistance(ant.Transform) < r2.Transform.GetDistance(ant.Transform)
                    ? r1
                    : r2);

                if (closest != null)
                {
                    if (ant.PickUp(closest))
                        stateEntity.State = Next(stateEntity);
                    else
                        stateEntity.State = new TargetState(this, closest);
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
            
            
            if (ant.SearchTime >= ant.SearchTimeout)
            {
                stateEntity.State = Next(stateEntity);
            }
            else
            {
                ant.SearchTime++;
            }
        }

        public override IState Next(StateEntity stateEntity)
        {
            return CarryState.Instance;
        }
    }
}