namespace AntEngine.Entities.States.Living
{
    public class CarryState : IState
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
        
        public void OnStateStart(StateEntity stateEntity)
        {
            throw new System.NotImplementedException();
        }

        public void OnStateUpdate(StateEntity stateEntity)
        {
            throw new System.NotImplementedException();
        }

        public void OnStateEnd(StateEntity stateEntity)
        {
            throw new System.NotImplementedException();
        }

        public IState Next(StateEntity stateEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}