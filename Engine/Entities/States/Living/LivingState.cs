using System;

namespace AntEngine.Entities.States.Living
{
    /// <summary>
    ///     Starting state of a Living Entity.
    /// </summary>
    public class LivingState : IState
    {
        private static LivingState _instance;

        public static LivingState Instance
        {
            get
            {
                _instance ??= new LivingState();
                return _instance;
            }
        }

        public virtual void OnStateStart(StateEntity stateEntity)
        {
            if (stateEntity is not LivingEntity)
                throw new ArgumentException("LivingState is only defined for Living entities.");
        }

        public virtual void OnStateUpdate(StateEntity stateEntity)
        {
            LivingEntity living = (LivingEntity)stateEntity;
            if (living.Health <= 0) living.State = Next(stateEntity);
        }

        public virtual void OnStateEnd(StateEntity stateEntity)
        {
        }

        public virtual IState Next(StateEntity stateEntity)
        {
            return DeathState.Instance;
        }
    }
}