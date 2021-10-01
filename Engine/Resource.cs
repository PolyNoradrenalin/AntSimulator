namespace AntEngine
{
    public class Resource
    {
        private int _amount;

        public Resource(int a)
        {
            _amount = a;
        }

        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }
    }
}