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

namespace Alternet.Syntax.Parsers.Lsp.Fx
{
    public class FxServerProcess
        : ServerProcess
    {
        // The scope factory.
        // This lets the LanguageServer object get the symbols in the RecalcEngine to use for intellisense.
        private readonly PowerFxScopeFactory scopeFactory = new PowerFxScopeFactory();

        private readonly StreamOverStream inputStream = new StreamOverStream();
        private readonly StreamOverStream outputStream = new StreamOverStream();

        /// <summary>
        ///     Create a new <see cref="FxServerProcess"/>.
        /// </summary>
        /// <param name="loggerFactory">
        ///     The factory for loggers used by the process and its components.
        /// </param>
        public FxServerProcess(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            inputStream.AfterWrite += InputStream_AfterWrite;
            outputStream.AfterRead += OutputStream_AfterRead;
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

        private void OutputStream_AfterRead(object sender, StreamReadWriteEventArgs e)
        {
        }

        private void InputStream_AfterWrite(object sender, StreamReadWriteEventArgs e)
        {
            string s = System.Text.Encoding.UTF8.GetString(e.Buffer, 0, e.Count);
            ProcessData(s);
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