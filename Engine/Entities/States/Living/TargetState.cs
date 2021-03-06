using System;
using System.Numerics;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Colonies;

namespace AntEngine.Entities.States.Living
{
    /// <summary>
    ///     Represents a state where the ant is targeting a specific entity until a minimum distance is reached.
    /// </summary>
    public class TargetState : LivingState
    {
        private const float ThresholdDistance = 1F;

        private readonly IState _previousState;
        private readonly Entity _target;

        public TargetState(IState previousState, Entity target)
        {
            _previousState = previousState;
            _target = target;
        }

        public override void OnStateStart(StateEntity stateEntity)
        {
            if (stateEntity is not Ant)
                throw new ArgumentException("TargetState is only valid on Ant entity");
        }

        /// <summary>
        ///     Moves towards a specified target in a straight line.
        /// </summary>
        /// <param name="stateEntity"></param>
        public override void OnStateUpdate(StateEntity stateEntity)
        {
            Ant ant = (Ant) stateEntity;
            Vector2 dir = _target.Transform.Position - ant.Transform.Position;

            ant.Move(dir);

            if (ant.LastEmitTime > ant.PheromoneEmissionDelay && ant.SearchTime < ant.SearchTimeout)
            {
                switch (_target)
                {
                    case ResourceEntity:
                        ant.EmitHomePheromone();
                        break;
                    case Colony:
                        ant.EmitFoodPheromone();
                        break;
                }

                ant.LastEmitTime = 0;
            }
            else
            {
                ant.LastEmitTime++;
            }

            if (ant.Transform.GetDistance(_target.Transform) <= ThresholdDistance) ant.State = Next(stateEntity);
        }

        public override IState Next(StateEntity stateEntity)
        {
            return _previousState;
        }
    }
}