using System;

namespace ApiCommon
{
    public class ManagedNameAttribute : Attribute
    {
        public ManagedNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}