using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Alternet.UI.Integration.Remoting;
using Task = System.Threading.Tasks.Task;

namespace Alternet.UI.Integration
{
    /// <summary>
    /// Manages running a XAML previewer process.
    /// </summary>
    public class PreviewerProcess : IDisposable
    {
        private string _assemblyPath;
        private string _executablePath;
        private string _hostAppPath;
        private double _scaling;
        private Process _process;
        private IAlternetUIRemoteTransportConnection _connection;
        private IDisposable _listener;
        private PreviewData _previewData;
        private ExceptionDetails _error;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewerProcess"/> class.
        /// </summary>
        public PreviewerProcess()
        {
        }

        public PreviewData PreviewData
        {
            get
            {
                return _previewData;
            }

            set
            {
                _previewData = value;
                PreviewDataReceived?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the current error state as returned from the previewer process.
        /// </summary>
        public ExceptionDetails Error
        {
            get => _error;
            private set
            {
                if (!Equals(_error, value))
                {
                    _error = value;
                    ErrorChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the previewer process is currently running.
        /// </summary>
        public bool IsRunning => _process != null && !_process.HasExited;

        /// <summary>
        /// Gets a value indicating whether the previewer process is ready to receive messages.
        /// </summary>
        public bool IsReady => IsRunning && _connection != null;

        /// <summary>
        /// Gets scaling for the preview.
        /// </summary>
        public double Scaling => _scaling;

        /// <summary>
        /// Raised when the <see cref="Error"/> state changes.
        /// </summary>
        public event EventHandler ErrorChanged;

        /// <summary>
        /// Raised when a new frame is available in <see cref="Bitmap"/>.
        /// </summary>
        public event EventHandler PreviewDataReceived;

        /// <summary>
        /// Raised when the underlying system process exits.
        /// </summary>
        public event EventHandler ProcessExited;

        public void LogError(string title, Exception exception, int lineNumber, int linePos)
        {
            Log.Error($"{title}: {exception}");
        }

        /// <summary>
        /// Starts the previewer process.
        /// </summary>
        /// <param name="assemblyPath">The path to the assembly containing the XAML.</param>
        /// <param name="executablePath">The path to the executable to use for the preview.</param>
        /// <param name="hostAppPath">The path to the host application.</param>
        /// <returns>A task tracking the startup operation.</returns>
        public async Task StartAsync(
            string assemblyPath,
            string executablePath,
            string hostAppPath)
        {
            Log.Verbose("Started PreviewerProcess.StartAsync()");

            hostAppPath = Path.ChangeExtension(hostAppPath, ".exe");

            if (_listener != null)
            {
                Log.Error("Previewer process already started.");
                return;
            }

            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                Log.Error("Assembly path may not be null or an empty string.");
                return;
            }

            if (string.IsNullOrWhiteSpace(executablePath))
            {
                Log.Error("Executable path may not be null or an empty string.");
                return;
            }

            var newHostAppPath = @"C:\AlternetUI\UIXmlHostApp\Alternet.UI.Integration.UIXmlHostApp.exe";

            if (File.Exists(newHostAppPath))
            {
                hostAppPath = newHostAppPath;
            }

            /*
            newHostAppPath
                = @"E:\DIMA\AlternetUI\Source\Integration\Components\Alternet.UI.Integration.UIXmlHostApp\bin\Debug\net8.0\Alternet.UI.Integration.UIXmlHostApp.exe";

            if (File.Exists(newHostAppPath))
            {
                hostAppPath = newHostAppPath;
            }
            */

            if (string.IsNullOrWhiteSpace(hostAppPath))
            {
                Log.Error("HostAppPath path may not be null or an empty string.");
                return;
            }

            if (!File.Exists(hostAppPath))
            {
                Log.Error($"HostApp not found. Path: {hostAppPath}");
                return;
            }

            if (!File.Exists(assemblyPath))
            {
                Log.Error($"Could not find '{assemblyPath}'. " +
                    $"Could not find '{assemblyPath}'. " +
                    "Please build your project to enable previewing and intellisense.");
                return;
            }

            if (!File.Exists(executablePath))
            {
                Log.Error($"Could not find executable '{executablePath}'. " + 
                    "Please build your project to enable previewing and intellisense.");
                return;
            }

            if (!File.Exists(hostAppPath))
            {
                Log.Error($"Could not find executable '{hostAppPath}'. " +
                    $"Could not find executable '{hostAppPath}'. " +
                    "Please build your project to enable previewing and intellisense.");
                return;
            }

            Log.Information($"AssemblyPath: {assemblyPath}");
            Log.Information($"ExecutablePath: {executablePath}");
            Log.Information($"HostAppPath: {hostAppPath}");

            _assemblyPath = assemblyPath;
            _executablePath = executablePath;
            _hostAppPath = hostAppPath;
            Error = null;

            var port = FreeTcpPort();
            var tcs = new TaskCompletionSource<object>();

            _listener = new BsonTcpTransport().Listen(
                IPAddress.Loopback,
                port,
#pragma warning disable
                async t =>
                {
                    try
                    {
                        await ConnectionInitializedAsync(t);
                        tcs.TrySetResult(null);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Error initializing connection: {ex}");
                        tcs.TrySetException(ex);
                    }
                });
#pragma warning restore

            /*
            var executableDir = Path.GetDirectoryName(_executablePath);
            var extensionDir = Path.GetDirectoryName(GetType().Assembly.Location);
            var targetName = Path.GetFileNameWithoutExtension(_executablePath);
            var runtimeConfigPath = Path.Combine(executableDir, targetName + ".runtimeconfig.json");
            var depsPath = Path.Combine(executableDir, targetName + ".deps.json");
            */

            //EnsureExists(runtimeConfigPath);
            //EnsureExists(depsPath);

            bool isDotNetCore = hostAppPath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase);

            isDotNetCore = false;

            string args;

            if (isDotNetCore)
                args = $@"exec ""{hostAppPath}"" --transport tcp-bson://127.0.0.1:{port}/ ""{_executablePath}""";
            else
                args = $@" --transport tcp-bson://127.0.0.1:{port}/ ""{_executablePath}""";

            var processInfo = new ProcessStartInfo
            {
                Arguments = args,
                CreateNoWindow = true,
                FileName = isDotNetCore ? "dotnet" : hostAppPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            Log.Information($"Starting previewer process for '{_executablePath}'");
            Log.Information($"App: {hostAppPath}");
            Log.Information($"Args: {args}");

            var process = _process = Process.Start(processInfo);
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += OnProcessOutputReceived;
            process.ErrorDataReceived += OnProcessErrorReceived;
            process.Exited += Abort;
            process.Exited += OnProcessExited;
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            void Abort(object sender, EventArgs e)
            {
                Log.Information("Process exited while waiting for connection init.");
                tcs.TrySetException(
                    new ApplicationException($"The previewer exited unexpectedly with code {process.ExitCode}."));
            }

            try
            {
                Log.Information(
                    $"Started previewer process for '{_executablePath}'. Waiting for connection init.");
                await tcs.Task;
            }
            finally
            {
                process.Exited -= Abort;
            }

            Log.Verbose("Finished PreviewerProcess.StartAsync()");
        }

        /// <summary>
        /// Stops the previewer process.
        /// </summary>
        public void Stop()
        {
            Log.Verbose("Started PreviewerProcess.Stop()");
            Log.Information("Stopping previewer process");

            _listener?.Dispose();
            _listener = null;

            if (_connection != null)
            {
                _connection.OnMessage -= ConnectionMessageReceived;
                _connection.OnException -= ConnectionExceptionReceived;
                _connection.Dispose();
                _connection = null;
            }

            if (_process != null && !_process.HasExited)
            {
                Log.Debug("Killing previewer process");

                try
                {
                    // Kill the process. Do not set _process to null here,
                    // wait for ProcessExited to be called.
                    _process.Kill();
                }
                catch (InvalidOperationException ex)
                {
                    Log.Debug($"Failed to kill previewer process: {ex}");
                }
            }

            _executablePath = null;
            _hostAppPath = null;

            Log.Verbose("Finished PreviewerProcess.Stop()");
        }

        /// <summary>
        /// Sets the scaling for the preview.
        /// </summary>
        /// <param name="scaling">The scaling factor.</param>
        /// <returns>A task tracking the operation.</returns>
        public async Task SetScalingAsync(double scaling)
        {
            _scaling = scaling;

            if (IsReady)
            {
                await SendAsync(new ClientRenderInfoMessage
                {
                    DpiX = 96 * _scaling,
                    DpiY = 96 * _scaling,
                });
            }
        }

        public long MaxProcessMemoryBytes = 200 * 1024 * 1024;

        /// <summary>
        /// Updates the XAML to be previewed.
        /// </summary>
        /// <param name="xaml">The XAML.</param>
        /// <returns>A task tracking the operation.</returns>
        public async Task UpdateXamlAsync(string xaml, System.Drawing.Point ownerWindowLocation)
        {
            if (_process == null)
                throw new InvalidOperationException("Process not started.");

            if (_connection == null)
                throw new InvalidOperationException("Process not finished initializing.");

            await RestartIfMaxMemoryReachedAsync();

            await SendAsync(new UpdateXamlMessage
            {
                AssemblyPath = _assemblyPath,
                Xaml = xaml,
                OwnerWindowX = ownerWindowLocation.X,
                OwnerWindowY = ownerWindowLocation.Y,
            });
        }

        private async Task RestartIfMaxMemoryReachedAsync()
        {
            _process.Refresh();
            if (_process.PrivateMemorySize64 > MaxProcessMemoryBytes)
            {
                Log.Information("StopIfMaxMemoryReached: restarting process.");

                await RestartAsync();
            }
        }

        private async Task RestartAsync()
        {
            var assemblyPath = _assemblyPath;
            var executablePath = _executablePath;
            var hostAppPath = _hostAppPath;

            Stop();
            await StartAsync(assemblyPath, executablePath, hostAppPath);

            if (_process == null)
                throw new InvalidOperationException("Process not started.");

            if (_connection == null)
                throw new InvalidOperationException("Process not finished initializing.");
        }

        /*/// <summary>
        /// Sends an input message to the process.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A task tracking the operation.</returns>
        public async Task SendInputAsync(InputEventMessageBase message)
        {
            if (_process == null)
            {
                throw new InvalidOperationException("Process not started.");
            }

            if (_connection == null)
            {
                throw new InvalidOperationException("Process not finished initializing.");
            }

            await SendAsync(message);
        }*/

        /// <summary>
        /// Stops the process and disposes of all resources.
        /// </summary>
        public void Dispose() => Stop();

        internal static void EnsureExists(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Could not find '{path}'.");
            }
        }

        private async Task ConnectionInitializedAsync(IAlternetUIRemoteTransportConnection connection)
        {
            Log.Verbose("Started PreviewerProcess.ConnectionInitializedAsync()");
            Log.Information("Connection initialized");

            if (!IsRunning)
            {
                Log.Verbose("ConnectionInitializedAsync detected process has stopped: aborting");
                return;
            }

            _connection = connection;
            _connection.OnException += ConnectionExceptionReceived;
            _connection.OnMessage += ConnectionMessageReceived;

            await SendAsync(new ClientSupportedPixelFormatsMessage
            {
                Formats = new[]
                {
                    Remoting.PixelFormat.Bgra8888,
                    Remoting.PixelFormat.Rgba8888,
                }
            });

            await SetScalingAsync(_scaling);

            Log.Verbose("Finished PreviewerProcess.ConnectionInitializedAsync()");
        }

        private async Task SendAsync(object message)
        {
            Log.Debug($"=> Sending {message}");
            await _connection.Send(message);
        }

        private async Task OnMessageAsync(object message)
        {
            Log.Verbose("Started PreviewerProcess.OnMessageAsync()");
            Log.Debug($"<= {message}");

            switch (message)
            {
                case PreviewDataMessage frame:
                    {
                        PreviewData = null;
                        PreviewData = new PreviewData(frame.ImageFileName);
                        Error = null;

                        await SendAsync(new PreviewDataReceivedMessage
                        {
                            SequenceId = frame.SequenceId
                        });
                        break;
                    }
                case UpdateXamlResultMessage update:
                {
                    var exception = update.Exception;

                    if (exception == null && !string.IsNullOrWhiteSpace(update.Error))
                    {
                        exception = new ExceptionDetails { Message = update.Error };
                    }

                    Error = exception;

                    if (exception != null)
                    {
                        LogError(
                            "UpdateXamlResult error",
                            new Exception(exception.Message + "\n" + exception.StackTrace),
                            exception.UixmlLineNumber ?? 0,
                            exception.UixmlLinePosition ?? 0);
                    }

                    break;
                }
            }

            Log.Verbose("Finished PreviewerProcess.OnMessageAsync()");
        }

        private void ConnectionMessageReceived(
            IAlternetUIRemoteTransportConnection connection,
            object message)
        {
            OnMessageAsync(message).FireAndForget();
        }

        private void ConnectionExceptionReceived(
            IAlternetUIRemoteTransportConnection connection,
            Exception ex)
        {
            Log.Error($"Connection error: {ex}");
        }

        private void OnProcessOutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                Log.Debug($"<= {e.Data}");
            }
        }

        private void OnProcessErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                Log.Error($"<= {e.Data}");
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            Log.Information("Process exited");
            Stop();
            ProcessExited?.Invoke(this, EventArgs.Empty);
        }

        private static int FreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        private static bool Equals(ExceptionDetails a, ExceptionDetails b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            return a?.ExceptionType == b?.ExceptionType &&
                a?.Message == b?.Message &&
                a?.UixmlLineNumber == b?.UixmlLineNumber &&
                a?.UixmlLinePosition == b?.UixmlLinePosition;
        }
    }
}