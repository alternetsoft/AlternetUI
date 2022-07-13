using System;
using System.Threading.Tasks;

namespace Alternet.UI.Integration.Remoting
{
    public interface IAlternetUIRemoteTransportConnection : IDisposable
    {
        Task Send(object data);
        event Action<IAlternetUIRemoteTransportConnection, object> OnMessage;
        event Action<IAlternetUIRemoteTransportConnection, Exception> OnException;
    }
}
