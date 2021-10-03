namespace AntEngine.Entity
{
    /// <summary>
    /// Represents a resource in the world.
    /// </summary>
    public class ResourceEntity : Entity
    {
        public ResourceEntity(int quantity, Resource.Resource resource)
        {
            Quantity = quantity;
            Resource = resource;
        }

        public int Quantity { get; set; }
        public Resource.Resource Resource { get;  set; }

        public override void Update()
        {
            
        }
    }
}