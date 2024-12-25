using System;

namespace ApiCommon
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class PInvokeAttributesAttribute : Attribute
    {
        public PInvokeAttributesAttribute(string attributesString)
        {
            AttributesString = attributesString;
        }

        public string AttributesString { get; }
    }
}