using System;

namespace ApiCommon
{
    [AttributeUsage(AttributeTargets.All)]
    public class NativeNameAttribute : Attribute
    {
        public NativeNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}