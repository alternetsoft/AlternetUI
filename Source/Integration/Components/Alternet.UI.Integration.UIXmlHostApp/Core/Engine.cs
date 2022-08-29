using Alternet.Drawing;
using Alternet.UI.Integration.Remoting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Alternet.UI.Integration.UIXmlHostApp.Remote
{
    public class Engine
    {
        private IAlternetUIRemoteTransportConnection transport;
        private string sessionId;

        private Application application;

        private Queue<Action> actions = new Queue<Action>();

        public Engine(IAlternetUIRemoteTransportConnection transport, string sessionId)
        {
            this.transport = transport;
            this.sessionId = sessionId;
        }

        public void Run()
        {
#if NETCOREAPP
            System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += AssemblyLoadContext_Resolving;
#endif

            EnsureApplicationCreated();

            UixmlLoader.DisableComponentInitialization = true;
            InternalsAccessor.SetUixmlPreviewerMode(application);

            transport.OnMessage += OnTransportMessage;
            transport.Start();
            Logger.Instance.Information("Sending StartDesignerSessionMessage");
            transport.Send(new StartDesignerSessionMessage { SessionId = sessionId });

            InternalsAccessor.AttachEventHandler(application, "Idle", Application_Idle);

            var timer = new Timer(TimeSpan.FromMilliseconds(100), OnQueueTimerTick);

            new System.Threading.Timer(_ => InternalsAccessor.WakeUpIdle(application), null, 0, 200);

            application.Run(new Window { IsToolWindow = true, StartLocation = WindowStartLocation.Manual, Location = new Point(20000, 20000) });
        }

        private void OnQueueTimerTick(object sender, EventArgs e)
        {
            ProcessQueue();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            ProcessQueue();
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

        private void EnsureApplicationCreated()
        {
            if (application == null)
                application = new Application();
        }

#if NETCOREAPP

        private Assembly AssemblyLoadContext_Resolving(System.Runtime.Loader.AssemblyLoadContext context, AssemblyName name)
        {
            if (name.Name == "Alternet.UI")
                return typeof(Alternet.UI.Window).Assembly;

            if (name.Name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
            {
                // See https://github.com/dotnet/runtime/issues/7077
                return null;
            }

            return context.LoadFromAssemblyName(name);
        }

#endif

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
                ProcessUixmlUpdate(uixml);
            }
        });

        private void ProcessUixmlUpdate(UpdateXamlMessage uixml)
        {
            try
            {
                var control = LoadControlFromUixml(uixml);
                var window = GetUixmlControlWindow(control);

                window.Show();

                var timer = new Timer(TimeSpan.FromMilliseconds(10));

                timer.Tick += (o, e) =>
                {
                    MoveWindowToScreenUnderneath(window, new System.Drawing.Point(uixml.OwnerWindowX, uixml.OwnerWindowY));

                    timer.Stop();
                    timer.Dispose();

                    var screenshotFileName = SaveWindowScreenshotToFile(window);

                    var width = (int)window.Width;
                    var height = (int)window.Height;

                    window.Close();
                    window.Dispose();

                    transport.Send(
                        new PreviewDataMessage()
                        {
                            ImageFileName = screenshotFileName,
                            DesiredWidth = width,
                            DesiredHeight = height
                        });
                };

                timer.Start();
            }
            catch (Exception e)
            {
                Logger.Instance.Error(e.ToString());
                transport.Send(new UpdateXamlResultMessage
                {
                    Error = "Fail",
                    Exception = new ExceptionDetails(e),
                });
            }
        }

        private string SaveWindowScreenshotToFile(Window window)
        {
            var targetDirectoryPath = ResourceLocator.ScreenshotsDirectory;
            var targetFilePath = Path.Combine(targetDirectoryPath, Guid.NewGuid().ToString("N") + ".bmp");
            InternalsAccessor.SaveScreenshot(window, targetFilePath);
            return targetFilePath;
        }

        private void MoveWindowToScreenUnderneath(Window window, System.Drawing.Point ownerWindowLocation)
        {
            var hwnd = InternalsAccessor.GetHandle(window);
            PInvoke.User32.GetWindowRect(hwnd, out var rect);
            var size = new System.Drawing.Size(rect.right - rect.left, rect.bottom - rect.top);

            PInvoke.User32.SetWindowPos(
                hwnd,
                new IntPtr(1),
                ownerWindowLocation.X,
                ownerWindowLocation.Y,
                size.Width,
                size.Height,
                PInvoke.User32.SetWindowPosFlags.SWP_NOACTIVATE);

            PInvoke.User32.UpdateWindow(hwnd);
        }

        private Control LoadControlFromUixml(UpdateXamlMessage xaml)
        {
            var appAssembly = Assembly.Load(File.ReadAllBytes(xaml.AssemblyPath)); // Load bytes to avoid locking the file.
            Control control;
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(xaml.Xaml)))
            {
                control = (Control)new UixmlLoader().Load(stream, appAssembly);
            }

            return control;
        }

        private Window GetUixmlControlWindow(Control control)
        {
            Window window;
            if (control is Window w)
                window = w;
            else
            {
                window = new Window();
                window.Children.Add(control);
            }

            SetOffscreenToolWindowProperties(window);
            return window;
        }

        private void SetOffscreenToolWindowProperties(Window window)
        {
            window.IsToolWindow = true;
            window.StartLocation = WindowStartLocation.Manual;
            window.Location = new Point(20000, 20000);
        }
    }
}