﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xaml;
using Alternet.UI.Integration.Remoting;
using Alternet.UI.Integration.VisualStudio.Models;
using Microsoft.VisualStudio.Shell;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Task = System.Threading.Tasks.Task;

namespace Alternet.UI.Integration.VisualStudio.Services
{
    /// <summary>
    /// Manages running a XAML previewer process.
    /// </summary>
    public class PreviewerProcess : IDisposable, ILogEventEnricher
    {
        private readonly ILogger _log;
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
            _log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Destructure.ToMaximumStringLength(32)
                .Enrich.With(this)
                .WriteTo.Logger(Log.Logger)
                .CreateLogger();
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

        public void LogError<T>(string messageTemplate, T propertyValue)
        {
            _log.Error<T>(messageTemplate, propertyValue);
        }

        public void LogError(Exception exception, string messageTemplate)
        {
            _log.Error(exception, messageTemplate);
        }

        public void LogErrorVS(Exception exception, string messageTemplate)
        {
            _log.Error(exception, messageTemplate);

            if(exception is XamlException xamplE)
            {
                var lineNumber = xamplE.LineNumber;
                var linePos = xamplE.LinePosition;
            }

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
            _log.Verbose("Started PreviewerProcess.StartAsync()");

            if (_listener != null)
            {
                throw new InvalidOperationException("Previewer process already started.");
            }

            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                throw new ArgumentException(
                    "Assembly path may not be null or an empty string.",
                    nameof(assemblyPath));
            }

            if (string.IsNullOrWhiteSpace(executablePath))
            {
                throw new ArgumentException(
                    "Executable path may not be null or an empty string.",
                    nameof(executablePath));
            }

            if (string.IsNullOrWhiteSpace(hostAppPath))
            {
                throw new ArgumentException(
                    "Executable path may not be null or an empty string.",
                    nameof(executablePath));
            }

            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException(
                    $"Could not find '{assemblyPath}'. " +
                    "Please build your project to enable previewing and intellisense.");
            }

            if (!File.Exists(executablePath))
            {
                throw new FileNotFoundException(
                    $"Could not find executable '{executablePath}'. " + 
                    "Please build your project to enable previewing and intellisense.");
            }

            if (!File.Exists(hostAppPath))
            {
                throw new FileNotFoundException(
                    $"Could not find executable '{hostAppPath}'. " +
                    "Please build your project to enable previewing and intellisense.");
            }

            _assemblyPath = assemblyPath;
            _executablePath = executablePath;
            _hostAppPath = hostAppPath;
            Error = null;

            var port = FreeTcpPort();
            var tcs = new TaskCompletionSource<object>();

            _listener = new BsonTcpTransport().Listen(
                IPAddress.Loopback,
                port,
#pragma warning disable VSTHRD101
                async t =>
                {
                    try
                    {
                        await ConnectionInitializedAsync(t);
                        tcs.TrySetResult(null);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, "Error initializing connection");
                        tcs.TrySetException(ex);
                    }
                });
#pragma warning restore VSTHRD101

            var executableDir = Path.GetDirectoryName(_executablePath);
            var extensionDir = Path.GetDirectoryName(GetType().Assembly.Location);
            var targetName = Path.GetFileNameWithoutExtension(_executablePath);
            var runtimeConfigPath = Path.Combine(executableDir, targetName + ".runtimeconfig.json");
            var depsPath = Path.Combine(executableDir, targetName + ".deps.json");

            //EnsureExists(runtimeConfigPath);
            //EnsureExists(depsPath);

            bool isDotNetCore = hostAppPath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase);

            //var args = $@"exec --runtimeconfig ""{runtimeConfigPath}"" --depsfile ""{depsPath}"" ""{hostAppPath}"" --transport tcp-bson://127.0.0.1:{port}/ ""{_executablePath}""";

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

            _log.Information("Starting previewer process for '{ExecutablePath}'", _executablePath);
            _log.Debug("> dotnet.exe {Args}", args);

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
                _log.Information("Process exited while waiting for connection to be initialized.");
                tcs.TrySetException(new ApplicationException($"The previewer process exited unexpectedly with code {process.ExitCode}."));
            }

            try
            {
                _log.Information("Started previewer process for '{ExecutablePath}'. Waiting for connection to be initialized.", _executablePath);
                await tcs.Task;
            }
            finally
            {
                process.Exited -= Abort;
            }

            _log.Verbose("Finished PreviewerProcess.StartAsync()");
        }

        /// <summary>
        /// Stops the previewer process.
        /// </summary>
        public void Stop()
        {
            _log.Verbose("Started PreviewerProcess.Stop()");
            _log.Information("Stopping previewer process");

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
                _log.Debug("Killing previewer process");

                try
                {
                    // Kill the process. Do not set _process to null here, wait for ProcessExited to be called.
                    _process.Kill();
                }
                catch (InvalidOperationException ex)
                {
                    _log.Debug(ex, "Failed to kill previewer process");
                }
            }

            _executablePath = null;
            _hostAppPath = null;

            _log.Verbose("Finished PreviewerProcess.Stop()");
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
                _log.Information("StopIfMaxMemoryReached: restarting process.");

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

        /// <summary>
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
        }

        /// <summary>
        /// Stops the process and disposes of all resources.
        /// </summary>
        public void Dispose() => Stop();

        void ILogEventEnricher.Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_process?.HasExited != true)
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Pid", _process?.Id ?? 0));
            }
        }

        private async Task ConnectionInitializedAsync(IAlternetUIRemoteTransportConnection connection)
        {
            _log.Verbose("Started PreviewerProcess.ConnectionInitializedAsync()");
            _log.Information("Connection initialized");

            if (!IsRunning)
            {
                _log.Verbose("ConnectionInitializedAsync detected process has stopped: aborting");
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

            _log.Verbose("Finished PreviewerProcess.ConnectionInitializedAsync()");
        }

        private async Task SendAsync(object message)
        {
            _log.Debug("=> Sending {@Message}", message);
            await _connection.Send(message);
        }

        private async Task OnMessageAsync(object message)
        {
            _log.Verbose("Started PreviewerProcess.OnMessageAsync()");
            _log.Debug("<= {@Message}", message);

            switch (message)
            {
                case PreviewDataMessage frame:
                    {
                        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                        PreviewData = null;
                        PreviewData = new PreviewData(frame.ImageFileName);

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
                        LogErrorVS(new XamlException(
                            exception.Message + "\n" + exception.StackTrace,
                            null,
                            exception.UixmlLineNumber ?? 0,
                            exception.UixmlLinePosition ?? 0),
                            "UpdateXamlResult error");
                    }

                    break;
                }
            }

            _log.Verbose("Finished PreviewerProcess.OnMessageAsync()");
        }

        private void ConnectionMessageReceived(IAlternetUIRemoteTransportConnection connection, object message)
        {
            OnMessageAsync(message).FireAndForget();
        }

        private void ConnectionExceptionReceived(
            IAlternetUIRemoteTransportConnection connection,
            Exception ex)
        {
            LogError(ex, "Connection error");
        }

        private void OnProcessOutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                _log.Debug("<= {Data}", e.Data);
            }
        }

        private void OnProcessErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                LogError("<= {Data}", e.Data);
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            _log.Information("Process exited");
            Stop();
            ProcessExited?.Invoke(this, EventArgs.Empty);
        }

        private static void EnsureExists(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Could not find '{path}'.");
            }
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