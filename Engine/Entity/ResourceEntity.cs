namespace AntEngine
{
    /// <summary>
    /// Represents a resource in the world.
    /// </summary>
    public class ResourceEntity : Entity.Entity
    {
        public ResourceEntity(int a)
        {
            Quantity = a;
        }

        public int Quantity { get; set; }

        public override void Update()
        {
            
        }
    }
}