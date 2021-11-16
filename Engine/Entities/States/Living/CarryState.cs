namespace AntEngine.Entities.States.Living
{
    public class CarryState : LivingState
    {
        private static CarryState _instance;

        public static CarryState Instance
        {
            get
            {
                _instance ??= new CarryState();
                return _instance;
            }
        }

        public override void OnStateUpdate(StateEntity stateEntity)
        {
            throw new System.NotImplementedException();
        }

        public new IState Next(StateEntity stateEntity)
        {
            return SearchState.Instance;
        }
    }
}