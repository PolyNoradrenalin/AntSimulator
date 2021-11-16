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
    public class SearchState : IState
    {
        private static SearchState _instance;

        public static SearchState Instance
        {
            get
            {
                _instance ??= new SearchState();
                return _instance;
            }
        }
        public void OnStateStart(StateEntity stateEntity)
        {
            if (stateEntity is not LivingEntity) 
                throw new System.ArgumentException("LivingState is only defined for Living entities.");
        }

        public void OnStateUpdate(StateEntity stateEntity)
        {
            LivingEntity living = (LivingEntity) stateEntity;
            if (living.Health <= 0)
            {
                living.State = Next(stateEntity);
            }

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

        public void OnStateEnd(StateEntity stateEntity)
        {
        }

        public IState Next(StateEntity stateEntity)
        {
            return CarryState.Instance;
        }
    }
}