using System;

namespace AntEngine.Resources
{
    /// <summary>
    /// Represents a resource type.
    /// </summary>
    public class Resource
    {
        public Resource(string type, string name)
        {
            Type = type;
            Name = name;
        }

        /// <summary>
        /// Unique identifier of the resource. <br />
        /// Resources can have the same name but still be different.
        /// </summary>
        public string Type { get; private set; }
        /// <summary>
        /// Display name of the resource.
        /// </summary>
        public string Name { get; private set; }
    }
}