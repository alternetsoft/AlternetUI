#region Copyright (c) 2016-2023 Alternet Software

/*
    AlterNET Code Editor Library

    Copyright (c) 2016-2023 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2023 Alternet Software

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Alternet.Common;

using Microsoft.Extensions.Logging;

using Microsoft.PowerFx.LanguageServerProtocol;

using Alternet.UI;
using StreamJsonRpc;
using StreamJsonRpc.Protocol;

namespace Alternet.Syntax.Parsers.Lsp.Fx
{
    public class FxServerProcess
        : ServerProcess
    {
        // The scope factory.
        // This lets the LanguageServer object get the symbols in the RecalcEngine to use for intellisense.
        private readonly PowerFxScopeFactory scopeFactory = new PowerFxScopeFactory();

        public readonly JsonMessageFormatter formatter = new JsonMessageFormatter(Encoding.UTF8);

        private readonly StreamOverStream inputStream;
        private readonly StreamOverStream outputStream;
        private readonly StreamOverStream serverInputStream;
        private readonly StreamOverStream serverOutputStream;
        private readonly Stream inputMemoryStream;
        private readonly Stream outputMemoryStream;
        private readonly HeaderDelimitedMessageHandler msgHandler;
        private readonly Stream syncInputStream;
        private readonly Stream syncOutputStream;
        private readonly LanguageServer languageServer;

        // The source for the StreamJsonRpc.JsonRpc.DisconnectedToken property.
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly CancellationTokenSource disconnectedSource = new CancellationTokenSource();

        // Gets a token that is cancelled when the connection is lost.
        internal CancellationToken DisconnectedToken => disconnectedSource.Token;

        /// <summary>
        ///     Create a new <see cref="FxServerProcess"/>.
        /// </summary>
        /// <param name="loggerFactory">
        ///     The factory for loggers used by the process and its components.
        /// </param>
        public FxServerProcess(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            inputMemoryStream = new MemoryStream();
            outputMemoryStream = new MemoryStream();
            syncInputStream = Stream.Synchronized(inputMemoryStream);
            syncOutputStream = Stream.Synchronized(inputMemoryStream);
            inputStream = new StreamOverStream(syncInputStream);
            outputStream = new StreamOverStream(syncOutputStream);
            serverInputStream = new StreamOverStream(syncInputStream);
            serverOutputStream = new StreamOverStream(syncOutputStream);

            inputStream.AfterWrite += InputStream_AfterWrite;
            outputStream.AfterRead += OutputStream_AfterRead;

            msgHandler = new HeaderDelimitedMessageHandler(
                serverOutputStream,
                serverInputStream,
                formatter);

            languageServer = new LanguageServer(SendToClient, scopeFactory);
        }

        public static void LogToFile(object? obj = null)
        {
            var msg = obj?.ToString() ?? string.Empty;
            var filename = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".log");

            string contents = $"{msg}{Environment.NewLine}";
            File.AppendAllText(filename, contents);
        }

        private void SendToClient(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            System.Buffers.ReadOnlySequence<byte> contentBuffer = new(bytes);

            JsonRpcMessage jsonRpcMessage =
                formatter.Deserialize(contentBuffer, Encoding.UTF8);

            LogToFile("==== SEND TO CLIENT");
            LogToFile(data);
            LogToFile("==== END SEND TO CLIENT");
            msgHandler.WriteAsync(jsonRpcMessage, DisconnectedToken);
        }

        /// <summary>
        ///     Is the server running?
        /// </summary>
        public override bool IsRunning => true/*!ServerExitCompletion.Task.IsCompleted*/;

        /// <summary>
        ///     The server's input stream.
        /// </summary>
        public override Stream InputStream
        {
            get
            {
                return inputStream;
                /*var stream = serverProcess.StandardInput;
                if (stream == null)
                    throw new InvalidOperationException();

                return stream.BaseStream;*/
            }
        }

        /// <summary>
        ///     The server's output stream.
        /// </summary>
        public override Stream OutputStream
        {
            get
            {
                return outputStream;
                /*var stream = serverProcess.StandardOutput;
                if (stream == null)
                    throw new InvalidOperationException();

                return stream.BaseStream;*/
            }
        }

        /// <summary>
        ///     Start or connect to the server.
        /// </summary>
        public override Task StartAsync()
        {
            ServerExitCompletion = new TaskCompletionSource<object>();

            //serverProcess.Exited += ServerProcess_Exit;

            ServerStartCompletion.TrySetResult(null!);

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Stop or disconnect from the server.
        /// </summary>
        public override async Task StopAsync()
        {
            ServerExitCompletion.TrySetResult(null!);

            /*var serverProcess = Interlocked.Exchange(ref this.serverProcess, null);
            if (serverProcess != null && !serverProcess.HasExited)
                serverProcess.Kill();*/

            await ServerExitCompletion.Task;
        }

        /// <summary>
        ///     Dispose of resources being used by the launcher.
        /// </summary>
        /// <param name="disposing">
        ///     Explicit disposal?
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                /*
                var serverProcess = Interlocked.Exchange(ref this.serverProcess, null);
                if (serverProcess != null)
                {
                    if (!serverProcess.HasExited)
                        serverProcess.Kill();

                    serverProcess.Dispose();
                }
                */
            }
        }

        public class RpcMessageProps
        {
            public string? jsonrpc { get; set; }

            public int? id { get; set; }

            public string method { get; set; }
        }

        public static string? GetMethodName(string s, out int id)
        {
            var obj = JsonSerializer.Deserialize<RpcMessageProps>(s);
            if (obj is null)
            {
                id = 0;
                return default;
            }

            var result = obj.method;
            id = obj.id ?? -1;
            return result;
        }

        private void OutputStream_AfterRead(object sender, StreamReadWriteEventArgs e)
        {
        }

        private void InputStream_AfterWrite(object sender, StreamReadWriteEventArgs e)
        {
            var jsonRpcMessage =
                msgHandler.ReadAsync(DisconnectedToken).ConfigureAwait(true).GetAwaiter().GetResult();

            var s = jsonRpcMessage.ToString();

            if (s is null)
                return;

            LogToFile(s);

            var methodName = GetMethodName(s, out var idValue);

            if (methodName == "initialize")
            {
                LogToFile("initialize");
                var result =
                    "{\"jsonrpc\":\"2.0\",\"id\":" + idValue.ToString() + ",\"result\":{\"capabilities\":{\"textDocumentSync\":2,\"hoverProvider\":true,\"completionProvider\":{\"resolveProvider\":true,\"triggerCharacters\":[\".\",\"-\",\":\",\"\\\\\"]},\"signatureHelpProvider\":{\"triggerCharacters\":[\" \"]},\"definitionProvider\":true,\"referencesProvider\":true,\"documentHighlightProvider\":true,\"documentSymbolProvider\":true,\"workspaceSymbolProvider\":true,\"codeActionProvider\":true,\"codeLensProvider\":{\"resolveProvider\":true},\"documentFormattingProvider\":false,\"documentRangeFormattingProvider\":false,\"documentOnTypeFormattingProvider\":null,\"renameProvider\":false,\"documentLinkProvider\":null,\"executeCommandProvider\":null,\"experimental\":null,\"foldingRangeProvider\":true}}}\r\n";
                SendToClient(result);
            }
            else
            if (methodName == "initialized")
            {
                LogToFile("initialized");
            }
            else
            {
                LogToFile("other");
                try
                {
                    languageServer.OnDataReceived(s);
                }
                catch (Exception ex)
                {
                    LogToFile(ex);
                    throw;
                }
            }
        }

        /*public static string GetWebStatusCodeString(HttpStatusCode statusCode, string statusDescription)
        {
            int num = (int)statusCode;
            string text = "(" + num.ToString(NumberFormatInfo.InvariantInfo) + ")";
            string text2 = null;
            try
            {
                text2 = SR.GetString("net_httpstatuscode_" + statusCode, null);
            }
            catch
            {
            }

            if (text2 != null && text2.Length > 0)
            {
                text = text + " " + text2;
            }
            else if (statusDescription != null && statusDescription.Length > 0)
            {
                text = text + " " + statusDescription;
            }

            return text;
        }*/

        private void ProcessData(string s)
        {
            var sendToClientData = new List<string>();
            var languageServer =
                new LanguageServer((string data) => sendToClientData.Add(data), scopeFactory);

            /*try
            {*/
            languageServer.OnDataReceived(s);

            var result = sendToClientData.ToArray();

            foreach (var item in result)
            {
                byte[] data = Encoding.UTF8.GetBytes(item);

                /*OutputStream.Write(data, 0, data.Length);*/
            }

            /*return GetWebStatusCodeString(200, sendToClientData.ToArray());*/
            /*}*/
            /*catch (Exception ex)
            {
                //return StatusCode(400, new { error = ex.Message });
            }*/
        }

        /// <summary>
        ///     Called when the server process has exited.
        /// </summary>
        /// <param name="sender">
        ///     The event sender.
        /// </param>
        /// <param name="args">
        ///     The event arguments.
        /// </param>
        /*private void ServerProcess_Exit(object sender, EventArgs args)
        {
            Log.LogDebug("Server process has exited.");

            OnExited();
            ServerExitCompletion.TrySetResult(null!);
            ServerStartCompletion = new TaskCompletionSource<object>();
        }*/
    }
}