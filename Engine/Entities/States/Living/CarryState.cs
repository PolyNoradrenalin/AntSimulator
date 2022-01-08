﻿using System;
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