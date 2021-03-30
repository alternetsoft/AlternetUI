using System;

namespace ApiCommon
{
    public class NativeNameAttribute : Attribute
    {
        public NativeNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}