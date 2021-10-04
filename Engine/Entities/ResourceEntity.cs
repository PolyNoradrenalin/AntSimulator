namespace AntEngine.Entities
{
    /// <summary>
    /// Represents a resource in the world.
    /// </summary>
    public class ResourceEntity : Entity
    {
        public ResourceEntity(int quantity, Resource.Resource resource)
        {
            Quantity = quantity;
            Type = resource;
        }

        public int Quantity { get; private set; }
        public Resource.Resource Type { get;  private set; }

        public override void Update()
        {
            
        }
    }
}