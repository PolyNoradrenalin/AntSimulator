using System.Transactions;
using AntEngine.Resources;

namespace AntEngine.Entities
{
    /// <summary>
    /// Represents a resource in the world.
    /// </summary>
    public class ResourceEntity : Entity
    {
        public ResourceEntity(World world, int quantity, Resource resource) : base(world)
        {
            Quantity = quantity;
            Type = resource;
        }

        public int Quantity { get; private set; }
        public Resource Type { get; private set; }

        public override void Update()
        {
            
        }
    }
}