using System;

namespace ApiCommon
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CallbackMarshalAttribute : Attribute
    {
        public CallbackMarshalAttribute(bool freeAfterFirstCall = true)
        {
            FreeAfterFirstCall = freeAfterFirstCall;
        }

        public bool FreeAfterFirstCall { get; }
    }
}