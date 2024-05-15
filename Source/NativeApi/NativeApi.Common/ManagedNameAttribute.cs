using System;

namespace ApiCommon
{
    [AttributeUsage(AttributeTargets.All)]
    public class ManagedNameAttribute : Attribute
    {
        public ManagedNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}