using System;

namespace Alternet.UI.Integration.Remoting
{
    public interface IMessageTypeResolver
    {
        Type GetByGuid(Guid id);
        Guid GetGuid(Type type);
    }
}