namespace AntEngine
{
    /// <summary>
    /// Represents a resource.
    /// </summary>
    public class Resource
    {
        public Resource(int a)
        {
            Quantity = a;
        }

        public int Quantity { get; set; }
    }
}