using System;

namespace AntEngine.Resource
{
    public class Resource
    {
        public Resource(String type)
        {
            Type = type;
        }

        public String Type { get; set; }
    }
}