namespace AntEngine.Resources
{
    /// <summary>
    ///     Represents a resource type.
    /// </summary>
    public class Resource
    {
        public Resource(string type, string name)
        {
            Type = type;
            Name = name;
        }

        /// <summary>
        ///     Unique identifier of the resource. <br />
        ///     Resources can have the same name but still be different.
        /// </summary>
        public string Type { get; }

        /// <summary>
        ///     Display name of the resource.
        /// </summary>
        public string Name { get; }

        protected bool Equals(Resource other)
        {
            return Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Resource) obj);
        }

        public override int GetHashCode()
        {
            return Type != null ? Type.GetHashCode() : 0;
        }
    }
}