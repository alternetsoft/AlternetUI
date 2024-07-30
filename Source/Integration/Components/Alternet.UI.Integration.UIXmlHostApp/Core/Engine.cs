using Alternet.UI.Integration.Remoting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    public class Engine
    {
        private readonly IAlternetUIRemoteTransportConnection transport;
        private readonly string sessionId;
        private readonly Assembly alternetUIAssembly;
        private readonly Queue<Action> actions = new();

        private dynamic previewerService;

        public Engine(
            IAlternetUIRemoteTransportConnection transport,
            string sessionId,
            Assembly alternetUIAssembly)
        {
            this.transport = transport;
            this.sessionId = sessionId;
            this.alternetUIAssembly = alternetUIAssembly;
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
            Log.Error(e.ToString());
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
            /*previewerService = new DummyService(
                (Action<IDictionary<string, object>>)OnUixmlUpdateSuccess,
                (Action<IDictionary<string, object>>)OnUixmlUpdateFailure,
                (Action)OnTick,
                ResourceLocator.ScreenshotsDirectory);*/

            previewerService = InternalsAccessor.CreateObject(
                alternetUIAssembly,
                "Alternet.UI.Integration.UIXmlPreviewerService",
                (Action<IDictionary<string, object>>)OnUixmlUpdateSuccess,
                (Action<IDictionary<string, object>>)OnUixmlUpdateFailure,
                (Action)OnTick,
                ResourceLocator.ScreenshotsDirectory);

            transport.OnMessage += OnTransportMessage;
            transport.Start();
            Log.Information("Sending StartDesignerSessionMessage");
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
                Log.Information("========");
                Log.Information("OnTransportMessage");

                Log.Information($"Uixml: {uixml.Xaml}");
                Log.Information($"AssemblyPath: {uixml.AssemblyPath}");
                Log.Information($"OwnerWindowX: {uixml.OwnerWindowX}");
                Log.Information($"OwnerWindowY: {uixml.OwnerWindowY}");
                Log.Information("========");

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