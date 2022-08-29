using System;
using System.Threading.Tasks;
using Alternet.UI.Integration.Remoting;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    class DetachableTransportConnection : IAlternetUIRemoteTransportConnection
    {
        private IAlternetUIRemoteTransportConnection _inner;

        public DetachableTransportConnection(IAlternetUIRemoteTransportConnection inner)
        {
            _inner = inner;
            _inner.OnMessage += FireOnMessage;
        }

        public void Dispose()
        {
            if (_inner != null)
                _inner.OnMessage -= FireOnMessage;
            _inner = null;
        }

        public void FireOnMessage(IAlternetUIRemoteTransportConnection transport, object obj) => OnMessage?.Invoke(transport, obj);
        
        public Task Send(object data)
        {
            return _inner?.Send(data);
        }

        public void Start()
        {
            _inner.Start();
        }

        public event Action<IAlternetUIRemoteTransportConnection, object> OnMessage;

        public event Action<IAlternetUIRemoteTransportConnection, Exception> OnException
        {
            add {}
            remove {}
        }
    }
}
