using System;

namespace Alternet.UI.Integration.Remoting
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AvaloniaRemoteMessageGuidAttribute : Attribute
    {
        public Guid Guid { get; }

        public AvaloniaRemoteMessageGuidAttribute(string guid)
        {
            Guid = Guid.Parse(guid);
        }
    }
}
