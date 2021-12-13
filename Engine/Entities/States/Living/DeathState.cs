using System;

namespace AntEngine.Entities.States.Living
{
    /// <summary>
    ///     Final state of a living entity.
    /// </summary>
    public class DeathState : IState
    {
        private static DeathState _instance;

        public static DeathState Instance
        {
            get
            {
                _instance ??= new DeathState();
                return _instance;
            }
        }

        public void OnStateStart(StateEntity stateEntity)
        {
            if (stateEntity is not LivingEntity living)
                throw new ArgumentException("LivingState is only defined for Living entities.");

            living.Kill();
        }

        public void OnStateUpdate(StateEntity stateEntity)
        {
        }

        public void OnStateEnd(StateEntity stateEntity)
        {
        }

        public IState Next(StateEntity stateEntity)
        {
            return null;
        }
    }
}