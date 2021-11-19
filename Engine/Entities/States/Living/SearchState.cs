using System.Collections.Generic;
using System.Linq;
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
            
            ant.EmitHomePheromone();
        }

        public new IState Next(StateEntity stateEntity)
        {
            return CarryState.Instance;
        }
    }
}