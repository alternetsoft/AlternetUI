using System;

namespace Alternet.UI.Integration.Remoting
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AlternetUIRemoteMessageGuidAttribute : Attribute
    {
        public Guid Guid { get; }

        public AlternetUIRemoteMessageGuidAttribute(string guid)
        {
            Guid = Guid.Parse(guid);
        }
    }
}
