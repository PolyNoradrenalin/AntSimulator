using System;

namespace AntEngine.Resources
{
    /// <summary>
    /// Represents a resource type.
    /// </summary>
    public class Resource
    {
        public Resource(String type)
        {
            Type = type;
        }
        
        public virtual String Name { get; set; }
        public String Type { get; private set; }
    }
}