using System;

namespace ApiCommon
{
    [AttributeUsage(AttributeTargets.All)]
    public class ManagedExternNameAttribute : Attribute
    {
        public ManagedExternNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}