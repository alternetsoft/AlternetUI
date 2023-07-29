using System;

namespace ApiCommon
{
    public class ManagedExternNameAttribute : Attribute
    {
        public ManagedExternNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}