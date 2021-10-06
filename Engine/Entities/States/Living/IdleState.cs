namespace AntEngine.Entities.States.Living
{
    /// <summary>
    /// Starting state of a Living Entity.
    /// </summary>
    public class IdleState : IState
    {
        private static IdleState _instance;

        public static IdleState Instance
        {
            get
            {
                _instance ??= new IdleState();
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
            LivingEntity living = (LivingEntity)stateEntity;
            if (living.Health <= 0)
            {
                living.ChangeState(Next(stateEntity));
            }
        }

        public void OnStateEnd(StateEntity stateEntity)
        {
        }

        public IState Next(StateEntity stateEntity)
        {
            return DeathState.Instance;
        }
    }
}