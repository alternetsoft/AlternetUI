using System;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using StreamJsonRpc;

namespace RpcStdioWorker
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--stdio")
            {
                // Ensure UTF-8 for console output
                Console.OutputEncoding = Encoding.UTF8;

                // Create a header-delimited message handler over stdin/stdout
                var input = Console.OpenStandardInput();
                var output = Console.OpenStandardOutput();

                var reader = PipeReader.Create(input);
                var writer = PipeWriter.Create(output);
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

                // HeaderDelimitedMessageHandler is a small helper in StreamJsonRpc that
                // implements the LSP-like "Content-Length" header framing.
                var handler = new HeaderDelimitedMessageHandler(pipe, new JsonMessageFormatter());

                // Create JsonRpc and register our target object (the methods callable by the client)
                var rpc = new JsonRpc(handler);
                var commandService = new CommandService();
                rpc.AddLocalRpcTarget(commandService);

                // Start listening for incoming requests from the client (stdin)
                rpc.StartListening();

                // Optionally write diagnostics to stderr so they are not mixed with the JSON-RPC channel.
                Console.Error.WriteLine("RPC (stdio) server started. Waiting for requests...");

                // Wait until the connection closes (client disconnects), or until rpc.Completion finishes.
                await rpc.Completion.ConfigureAwait(false);
                return 0;
            }
            else
            {
                Console.WriteLine("Usage: RpcStdioWorker --stdio");
                Console.WriteLine("When run with --stdio it listens for JSON-RPC messages on stdin/stdout.");
                return 1;
            }
        }
    }

    // The RPC request object shape that clients will send.
    public class CommandRequest
    {
        public string Command { get; set; } = "";
        // Optional: you can add WorkingDirectory, Environment, etc.
    }

    // The RPC response shape returned to the client.
    public class CommandResult
    {
        public int ExitCode { get; set; }
        public string StdOut { get; set; } = "";
        public string StdErr { get; set; } = "";
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

    // The service object that exposes methods callable by JSON-RPC clients.
    // Public methods will be callable (by default the method name is the method name).
    public class CommandService
    {
        // JSON-RPC method: "Execute"
        // When the client calls method "Execute" with CommandRequest params, this method executes.
        public async Task<CommandResult> Execute(CommandRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Command))
                throw new ArgumentException("Command must be provided.", nameof(request));

            // Run the command via a shell to allow complex commands / built-ins.
            var psi = CreateShellProcessStartInfo(request.Command);

            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            using var process = new Process { StartInfo = psi, EnableRaisingEvents = true };

            var stdoutTask = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            var stderrTask = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            var exitTask = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);

            try
            {
                process.Start();

                // Read output asynchronously
                var readOut = process.StandardOutput.ReadToEndAsync();
                var readErr = process.StandardError.ReadToEndAsync();

                // Monitor exit in a task
                var waitForExit = Task.Run(() =>
                {
                    process.WaitForExit();
                    return process.ExitCode;
                });

                using (cancellationToken.Register(() =>
                {
                    try
                    {
                        if (!process.HasExited)
                        {
                            process.Kill(entireProcessTree: true);
                        }
                    }
                    catch
                    {
                        // swallow
                    }
                }))
                {
                    await Task.WhenAll(readOut, readErr, waitForExit).ConfigureAwait(false);

                    return new CommandResult
                    {
                        ExitCode = waitForExit.Result,
                        StdOut = readOut.Result,
                        StdErr = readErr.Result
                    };
                }
            }
            finally
            {
                try { process.Close(); } catch { }
            }
        }

        private static ProcessStartInfo CreateShellProcessStartInfo(string command)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Use cmd.exe /c on Windows
                return new ProcessStartInfo("cmd.exe", "/c " + command);
            }
            else
            {
                // Use bash -lc on Unix-like (bash is common on Linux/macOS)
                // Wrap command in quotes and escape inner quotes.
                var escaped = command.Replace("\"", "\\\"");
                return new ProcessStartInfo("/bin/bash", "-lc \"" + escaped + "\"");
            }
        }
    }
}