namespace AntEngine
{
    /// <summary>
    /// Represents a resource.
    /// </summary>
    public class Resource
    {
        private int _quantity;

        public Resource(int a)
        {
            _quantity = a;
        }

        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }
    }
}