using AntEngine.Resources;

namespace AntEngine.Entities
{
    /// <summary>
    /// Represents a resource in the world.
    /// </summary>
    public class ResourceEntity : Entity
    {
        public ResourceEntity(int quantity, Resource resource)
        {
            Quantity = quantity;
            Type = resource;
        }

        public int Quantity { get; set; }
        public Resource Type { get;  set; }

        public override void Update()
        {
            
        }
    }
}