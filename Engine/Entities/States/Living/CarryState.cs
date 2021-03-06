using System.Collections.Generic;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Colonies;
using AntEngine.Entities.Pheromones;
using AntEngine.Resources;

namespace AntEngine.Entities.States.Living
{
    /// <summary>
    ///     Represents a state where an ant will want to carry food back to it's home colony.
    ///     Will follow HomePheromones by using WanderState.
    /// </summary>
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

            // Checks if a colony is in the ant's surroundings
            HashSet<Colony> colonies = ant.GetSurroundingEntities<Colony>();

            // When we find a colony, we check if this is the Ant's colony and try to deposit all the resources.
            foreach (Colony c in colonies)
            {
                if (ant.Home != c) continue;

                // Go towards the colony if too far away.
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

                return;
            }

            // Emits food pheromones on a regular interval.
            if (ant.LastEmitTime > ant.PheromoneEmissionDelay && ant.SearchTime < ant.SearchTimeout)
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