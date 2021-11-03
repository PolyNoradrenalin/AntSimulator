using AntEngine.Entities.States;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities
{
    /// <summary>
    /// An entity that can be a finite set of states and change state with specific transitions.
    /// </summary>
    public abstract class StateEntity : Entity
    {
        public StateEntity(World world, IState initialState) : this("StateEntity", new Transform(), world, initialState)
        {
        }
        
        public StateEntity(string name, Transform transform, World world, IState initialState) : base(name, transform, world)
        {
            State = initialState;
        }

        /// <summary>
        /// Current state of the Entity.
        /// </summary>
        public IState State { get; protected set; }
        
        /// <summary>
        /// Updates the state of the entity.
        /// Will also call OnStateEnd on previous state and OnStateStart on the new state.
        /// </summary>
        /// <param name="state">The new state the entity will be in.</param>
        public void ChangeState(IState state)
        {
            if (state == null) return;
            
            State?.OnStateEnd(this);
            State = state;
            State.OnStateStart(this);
        }

        /// <summary>
        /// Calls the Update method of the current state. 
        /// </summary>
        public override void Update()
        {
            State.OnStateUpdate(this);
        }
    }
}