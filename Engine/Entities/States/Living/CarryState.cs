using System.Collections.Generic;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Colonies;
using AntEngine.Entities.Pheromones;
using AntEngine.Resources;

namespace AntEngine.Entities.States.Living
{
    public class CarryState : WanderState<HomePheromone>
    {
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

            HashSet<Colony> colonies = ant.GetSurroundingEntities<Colony>();

            // When we find a colony, we check if this is the Ant's colony and try to deposit all the resources.
            foreach (Colony c in colonies)
            {
                if (ant.Home != c) continue;

                if (ant.Transform.GetDistance(c.Transform) >= ant.PickUpDistance)
                {
                    ant.State = new TargetState(this, c);
                    return;
                }

                foreach ((Resource resource, int cost) in ant.ResourceInventory.All)
                {
                    c.Stockpile.AddResource(resource, cost);
                    ant.ResourceInventory.RemoveResource(resource, cost);
                }

                stateEntity.State = Next(stateEntity);

                break;
            }


            if (ant.LastEmitTime > ant.PheromoneEmissionDelay)
            {
                ant.EmitFoodPheromone();
                ant.LastEmitTime = 0;
            }
            else
            {
                ant.LastEmitTime++;
            }
        }

        public override IState Next(StateEntity stateEntity)
        {
            return SearchState.Instance;
        }
    }
}