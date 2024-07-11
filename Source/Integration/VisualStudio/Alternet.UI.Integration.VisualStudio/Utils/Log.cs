using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Alternet.UI.Integration.VisualStudio
{
    public static class Log
    {
        /*private static IVsOutputWindowPane pane;
        private static IServiceProvider _provider;
        private static Guid _guid;
        private static string _name;
        private static object _syncRoot = new object();*/

        private static CancellationToken cancellationToken = new();

        /*/// <summary>
        /// Initializes the logger.
        /// </summary>
        /// <param name="provider">The service provider or Package instance.</param>
        /// <param name="name">The name to use for the custom Output Window pane.</param>
        public static void Initialize(IServiceProvider provider, string name)
        {
            _provider = provider;
            _name = name;
        }*/

        public static void Information(string s)
        {
            Write($"Information: {s}");
        }

        public static void Error(string s)
        {
            Write($"Error: {s}");
        }

        public static void Verbose(string s)
        {
            Write($"Verbose: {s}");
        }

        [Conditional("DEBUG")]
        public static void Debug(string s)
        {
            Write($"Debug: {s}");
        }

        private static void Write(string message)
        {
            AlternetUIPackage.JTF.Run(() =>
            {
                return WriteAsync(message);
            });
        }

        private static async Task WriteAsync(string message)
        {
            var jtf = AlternetUIPackage.JTF;

            if (jtf is null)
                return;

            await jtf.SwitchToMainThreadAsync();

            AlternetUIPackage.OutputPane?.OutputStringThreadSafe(
                DateTime.Now + ": " + message + Environment.NewLine);
        }

        /*/// <summary>
        /// Removes all text from the Output Window pane.
        /// </summary>
        public static void Clear()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            pane?.Clear();
        }*/

        /*/// <summary>
        /// Deletes the Output Window pane.
        /// </summary>
        public static void DeletePane()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            if (pane != null)
            {
                try
                {
                    var output = (IVsOutputWindow)_provider.GetService(typeof(SVsOutputWindow));
                    output.DeletePane(ref _guid);
                    pane = null;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex);
                }
            }
        }*/

        /*private static bool EnsurePane()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (pane == null)
            {
                lock (_syncRoot)
                {
                    if (pane == null)
                    {
                        _guid = Guid.NewGuid();
                        IVsOutputWindow output = (IVsOutputWindow)_provider.GetService(typeof(SVsOutputWindow));
                        output.CreatePane(ref _guid, _name, 1, 1);
                        output.GetPane(ref _guid, out pane);
                    }
                }
            }

            return pane != null;
        }*/
    }
}