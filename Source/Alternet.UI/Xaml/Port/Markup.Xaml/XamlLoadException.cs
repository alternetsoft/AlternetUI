#nullable disable
using System;
using System.Runtime.Serialization;

namespace Alternet.UI.Markup.Xaml
{
    internal class XamlLoadException: Exception
    {
        public XamlLoadException()
        {
        }

        protected XamlLoadException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }

        public XamlLoadException(string message): base(message)
        {
        }

        public XamlLoadException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}
