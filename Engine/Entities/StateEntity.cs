using AntEngine.Entities.States;

namespace AntEngine.Entities
{
    /// <summary>
    /// An entity that can be a finite set of states and change state with specific transitions.
    /// </summary>
    public abstract class StateEntity
    {
        public StateEntity(IState initialState)
        {
            State = initialState;
        }

        private IState State { get; set; }
        
        /// <summary>
        /// Updates the state of the entity.
        /// </summary>
        /// <param name="state">The new state the entity will be in.</param>
        public void ChangeState(IState state)
        {
            if (state == null) return;
            
            State?.OnStateEnd(this);
            State = state;
            State.OnStateStart(this);
        }
    }
}