using System;
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

        public int Quantity { get; private set; }
        public Resource Type { get; }

        public void AddResource(int quantity)
        {
            Quantity += quantity;
        }

        public int RemoveResource(int quantity)
        {
            int newValue = Quantity - quantity;
            if (newValue <= 0)
            {
                quantity = Quantity;
            }

            Quantity -= quantity;
            if (Quantity <= 0) World.RemoveEntity(this);
            
            return quantity;
        }

        public override void Update()
        {
        }
    }
}