using System.Collections.Generic;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Colonies;
using AntEngine.Entities.Pheromones;
using AntEngine.Resources;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities.States.Living
{
    public class CarryState : LivingState
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
            PerceptionMap perceptionMap = ant.GetPerceptionMap<FoodPheromone>();

            ant.Move(ant.MovementStrategy.Move(perceptionMap));

            List<Entity> list = ant.GetSurroundingEntities<ResourceEntity>();

            foreach (Entity e in list)
            {
                if (e is Colony colony)
                {
                    foreach ((Resource resource, int cost) in ant.ResourceInventory.All)
                    {
                        colony.Stockpile.AddResource(resource, cost);
                        ant.ResourceInventory.RemoveResource(resource, cost);
                    }
                    
                    stateEntity.State = Next(stateEntity);

                    break;
                }
            }
            
            ant.EmitFoodPheromone();
        }

        public new IState Next(StateEntity stateEntity)
        {
            return SearchState.Instance;
        }
    }
}