using System;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using StreamJsonRpc;

namespace RpcStdioClient
{
    // Mirror of the server types (must match shapes)
    public class CommandRequest
    {
        public string Command { get; set; } = string.Empty;
    }

    public class CommandResult
    {
        public int ExitCode { get; set; }
        public string StdOut { get; set; } = string.Empty;
        public string StdErr { get; set; } = string.Empty;
    }

    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: RpcStdioClient <workerPathOrExeOrDll>");
                Console.WriteLine("Examples:");
                Console.WriteLine("  RpcStdioClient ./RpcStdioWorker.dll");
                Console.WriteLine("  RpcStdioClient C:\\path\\to\\RpcStdioWorker.exe");
                Console.WriteLine("If you pass a .dll the client runs: dotnet <dll> --stdio");
                return 2;
            }

            var workerPath = args[0];

            workerPath = Path.GetFullPath(workerPath, Directory.GetCurrentDirectory());

            // Build process start info: if the path ends with .dll, run via 'dotnet', otherwise run directly.
            var psi = new ProcessStartInfo();
            if (workerPath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                psi.FileName = "dotnet";
                psi.Arguments = $"\"{workerPath}\" --stdio";
            }
            else
            {
                psi.FileName = workerPath;
                psi.Arguments = "--stdio";
            }

            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            using var process = new Process { StartInfo = psi, EnableRaisingEvents = true };

            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to start worker process: {ex.Message}");
                return 3;
            }

            // Forward worker stderr to the client's stderr for diagnostics
            _ = Task.Run(async () =>
            {
                var buffer = new char[4096];
                try
                {
                    using var errReader = process.StandardError;
                    int read;
                    while ((read = await errReader.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                    {
                        Console.Error.Write(new string(buffer, 0, read));
                    }
                }
                catch
                {
                    // ignore on shutdown
                }
            });

            var reader = PipeReader.Create(process.StandardOutput!.BaseStream);
            var writer = PipeWriter.Create(process.StandardInput!.BaseStream);
            var pipe = new DuplexPipe(reader, writer);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var formatter = new SystemTextJsonFormatter
            {
                JsonSerializerOptions = options,
            };

            // Create header-delimited message handler:
            // note: the constructor takes (inputStream, outputStream)
            var messageHandler = new HeaderDelimitedMessageHandler(pipe, new JsonMessageFormatter());

            using var rpc = new JsonRpc(messageHandler);
            rpc.StartListening();

            Console.WriteLine("Connected to worker. Type commands to execute remotely. Type 'exit' or Ctrl+D to quit.");
            Console.WriteLine("Press Ctrl+C while waiting for a result to cancel the call.");

            // Handle Ctrl+C to exit the interactive loop completely
            var appCts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                // Do not terminate process immediately; let the interactive loop handle cancellation per-call.
                // If user presses Ctrl+C twice, allow termination
                e.Cancel = true;
                appCts.Cancel();
            };

            try
            {
                while (!appCts.IsCancellationRequested)
                {
                    Console.Write("> ");
                    string? line = Console.ReadLine();
                    if (line == null) // EOF (Ctrl+D on Unix)
                        break;

                    line = line.Trim();
                    if (string.Equals(line, "exit", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(line, "quit", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    if (line.Length == 0)
                        continue;

                    // Per-call cancellation token (user can press Ctrl+C to cancel the current call).
                    using var callCts = new CancellationTokenSource();
                    void CancelOnce(object? s, ConsoleCancelEventArgs e)
                    {
                        e.Cancel = true;
                        callCts.Cancel();
                    }

                    ConsoleCancelEventHandler? handler = CancelOnce;
                    Console.CancelKeyPress += handler;

                    try
                    {
                        var request = new CommandRequest { Command = line };

                        Console.WriteLine($"Sending command: {line}");
                        CommandResult result;
                        try
                        {
                            object[] arguments = new object[1] { request };
                            result = await rpc.InvokeWithCancellationAsync<CommandResult>("Execute", arguments, callCts.Token);

                        }
                        catch (OperationCanceledException)
                        {
                            Console.WriteLine("Request cancelled.");
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"RPC call failed: {ex.Message}");
                            continue;
                        }

                        Console.WriteLine($"ExitCode: {result.ExitCode}");
                        if (!string.IsNullOrEmpty(result.StdOut))
                        {
                            Console.WriteLine("--- stdout ---");
                            Console.WriteLine(result.StdOut.TrimEnd('\n', '\r'));
                        }
                        if (!string.IsNullOrEmpty(result.StdErr))
                        {
                            Console.WriteLine("--- stderr ---");
                            Console.Error.WriteLine(result.StdErr.TrimEnd('\n', '\r'));
                        }
                        Console.WriteLine("--------------");
                    }
                    finally
                    {
                        if (handler != null)
                            Console.CancelKeyPress -= handler;
                    }
                }
            }
            finally
            {
                // Graceful shutdown
                try
                {
                    // disposal of rpc will stop listening
                    rpc.Dispose();
                }
                catch { }

                // terminate worker if still running
                if (!process.HasExited)
                {
                    try
                    {
                        process.Kill(true);
                    }
                    catch { }
                }
            }

            Console.WriteLine("Client exiting.");
            return 0;
        }

        public class DuplexPipe : IDuplexPipe
        {
            public DuplexPipe(PipeReader input, PipeWriter output)
            {
                Input = input;
                Output = output;
            }

            public PipeReader Input { get; }

            public PipeWriter Output { get; }
        }
    }
}