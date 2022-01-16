using AntEngine.Resources;

namespace AntEngine.Entities
{
    /// <summary>
    ///     Represents a resource in the world.
    /// </summary>
    public class ResourceEntity : Entity
    {
        public ResourceEntity(World world, int quantity, Resource resource) : base(world)
        {
            Quantity = quantity;
            Type = resource;
        }

        /// <summary>
        ///     Quantity of the resource remaining.
        /// </summary>
        public int Quantity { get; private set; }
        
        /// <summary>
        ///     Type of the resource.
        /// </summary>
        public Resource Type { get; }

        /// <summary>
        ///     Adds a quantity of resource to this ResourceEntity.
        /// </summary>
        /// <param name="quantity">Amount to add</param>
        public void AddResource(int quantity)
        {
            Quantity += quantity;
        }

        /// <summary>
        ///     Removes a quantity of resource from this ResourceEntity.
        ///     If the amount to remove is more that the current quantity this method will only remove the current quantity.
        /// </summary>
        /// <param name="quantity">Amount to remove.</param>
        /// <returns>Amount removed.</returns>
        public int RemoveResource(int quantity)
        {
            if (Quantity - quantity <= 0)
            {
                World.RemoveEntity(this);
                return Quantity;
            }

            Quantity -= quantity;
            return quantity;
        }

        public override void Update()
        {
        }
    }
}