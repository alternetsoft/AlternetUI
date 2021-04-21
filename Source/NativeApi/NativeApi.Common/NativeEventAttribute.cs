using System;

namespace ApiCommon
{
    [AttributeUsage(AttributeTargets.Event)]
    public class NativeEventAttribute : Attribute
    {
        public NativeEventAttribute()
        {
        }

        public NativeEventAttribute(string paramsStructName)
        {
            ParamsStructName = paramsStructName;
        }

        public NativeEventAttribute(bool cancellable, string? paramsStructName = null)
        {
            Cancellable = cancellable;
            ParamsStructName = paramsStructName;
        }

        public string? ParamsStructName { get; }

        public bool Cancellable { get; }
    }
}