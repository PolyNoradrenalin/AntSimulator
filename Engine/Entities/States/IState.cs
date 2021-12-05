namespace AntEngine.Entities.States
{
    /// <summary>
    /// Represents what can a StateEntity should do in a specific situation.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Called when the state has just been assigned on an entity.
        /// </summary>
        /// <param name="stateEntity">The entity that transitioned to this state.</param>
        void OnStateStart(StateEntity stateEntity);

        /// <summary>
        /// Called on each update of a StateEntity.
        /// </summary>
        /// <param name="stateEntity">The updating entity.</param>
        void OnStateUpdate(StateEntity stateEntity);

        /// <summary>
        /// Called when the state is being removed from an entity.
        /// </summary>
        /// <param name="stateEntity">The entity ending this state.</param>
        void OnStateEnd(StateEntity stateEntity);

        /// <summary>
        /// Gives the next state.
        /// </summary>
        /// <param name="stateEntity">The context of the state.</param>
        IState Next(StateEntity stateEntity);
    }
}