using Alternet.UI.Integration.Remoting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    public class Engine
    {
        private readonly string uiAssemblyPath;
        private IAlternetUIRemoteTransportConnection transport;
        private string sessionId;
        private dynamic previewerService;

        private Queue<Action> actions = new Queue<Action>();

        public Engine(IAlternetUIRemoteTransportConnection transport, string sessionId, string uiAssemblyPath)
        {
            this.transport = transport;
            this.sessionId = sessionId;
            this.uiAssemblyPath = uiAssemblyPath;
        }

        public void OnUixmlUpdateSuccess(IDictionary<string, object> parameters)
        {
            transport.Send(
                new PreviewDataMessage()
                {
                    ImageFileName = (string)parameters["ImageFileName"],
                });
        }

        public void OnUixmlUpdateFailure(IDictionary<string, object> parameters)
        {
            var e = (Exception)parameters["Exception"];
            Logger.Instance.Error(e.ToString());
            transport.Send(new UpdateXamlResultMessage
            {
                Error = (string)parameters["Message"],
                Exception = new ExceptionDetails(e),
            });
        }

        public void OnTick()
        {
            ProcessQueue();
        }

        public void Run()
        {
            var assembly = Assembly.LoadFile(uiAssemblyPath);

            previewerService = InternalsAccessor.CreateObject(
                assembly,
                "Alternet.UI.Integration.UIXmlPreviewerService",
                (object)OnUixmlUpdateSuccess,
                (object)OnUixmlUpdateFailure,
                (object)OnTick,
                ResourceLocator.ScreenshotsDirectory);

            transport.OnMessage += OnTransportMessage;
            transport.Start();
            Logger.Instance.Information("Sending StartDesignerSessionMessage");
            transport.Send(new StartDesignerSessionMessage { SessionId = sessionId });

            previewerService.Run();
        }

        private void ProcessQueue()
        {
            lock (actions)
            {
                while (actions.Count > 0)
                {
                    actions.Dequeue()();
                }
            }
        }

        private void BeginInvoke(Action action)
        {
            lock (actions)
                actions.Enqueue(action);
        }

        private void OnTransportMessage(IAlternetUIRemoteTransportConnection transport, object obj) =>
            BeginInvoke(() =>
        {
            if (obj is UpdateXamlMessage uixml)
            {
                previewerService.ProcessUixmlUpdate(
                    new Dictionary<string, object>
                    {
                        { "Uixml", uixml.Xaml },
                        { "AssemblyPath", uixml.AssemblyPath },
                        { "OwnerWindowX", uixml.OwnerWindowX },
                        { "OwnerWindowY", uixml.OwnerWindowY }
                    });
            }
        });
    }
}