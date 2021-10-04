using System;

namespace AntEngine.Resource
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

        public String Type { get; private set; }
    }
}