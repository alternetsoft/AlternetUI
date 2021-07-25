using System;

namespace ApiCommon
{
    [AttributeUsage(AttributeTargets.Event)]
    public class NativeEventAttribute : Attribute
    {
        public NativeEventAttribute()
        {
        }

        public NativeEventAttribute(bool cancellable)
        {
            Cancellable = cancellable;
        }

        public bool Cancellable { get; }
    }
}