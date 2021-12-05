using AntEngine.Entities.States;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities
{
    /// <summary>
    /// An entity that can be a finite set of states and change state with specific transitions.
    /// </summary>
    public abstract class StateEntity : Entity
    {
        private IState _state;

        public StateEntity(World world, IState initialState) : this("StateEntity", new Transform(), world, initialState)
        {
        }

        public StateEntity(string name, Transform transform, World world, IState initialState) : base(name, transform,
            world)
        {
            State = initialState;
        }

        /// <summary>
        /// State of the entity.
        /// Will also call OnStateEnd on previous state and OnStateStart on the new state when it is changed.
        /// </summary>
        public IState State
        {
            get => _state;
            set
            {
                if (value == null) return;
                _state?.OnStateEnd(this);
                _state = value;
                _state.OnStateStart(this);
            }
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