using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// General application utility methods.
    /// </summary>
    public static class AppUtils
    {
        /// <summary>
        /// Uses <see cref="Process"/> to start the application.
        /// </summary>
        /// <param name="filePath">Path to the application.</param>
        /// <param name="args">Set of command-line arguments to use when starting the application.
        /// See <see cref="ProcessStartInfo.Arguments"/></param>
        /// <param name="folder">Initial directory.
        /// See <see cref="ProcessStartInfo.WorkingDirectory"/></param>
        /// <returns><c>true</c> if operation is successful; <c> false</c> otherwise.</returns>
        public static bool ProcessStart(string filePath, string? args = null, string? folder = null)
        {
            using Process process = new();
            process.StartInfo.Verb = "open";
            process.StartInfo.FileName = filePath;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            if (args is not null)
                process.StartInfo.Arguments = args;
            if (folder != null)
                process.StartInfo.WorkingDirectory = folder;
            try
            {
                process.Start();
                return true;
            }
            catch (Exception e)
            {
                LogUtils.LogException(e);
                return false;
            }
        }

        /// <summary>
        /// Opens url in the default browser.
        /// </summary>
        /// <param name="url">Url to open.</param>
        /// <returns><c>true</c> if operation is successful; <c> false</c> otherwise.</returns>
        public static bool OpenUrl(string url)
        {
            var result = ShellExecute(url);
            return result;
        }

        /// <inheritdoc cref="ProcessStart"/>
        /// <remarks>
        /// Uses shell execute to start the process.
        /// </remarks>
        public static bool ShellExecute(string filePath, string? args = null, string? folder = null)
        {
            using Process process = new();
            process.StartInfo.Verb = "open";
            process.StartInfo.FileName = filePath;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = true;
            if(args is not null)
                process.StartInfo.Arguments = args;
            if (folder != null)
                process.StartInfo.WorkingDirectory = folder;
            try
            {
                process.Start();
                return true;
            }
            catch (Exception e)
            {
                LogUtils.LogException(e);
                return false;
            }
        }

        /// <summary>
        /// Uses <see cref="Process"/> to start the application.
        /// </summary>
        /// <param name="startInfo">Process start parameters.</param>
        /// <returns></returns>
        public static bool ProcessStartEx(ProcessStartInfo startInfo)
        {
            using Process process = new();
            process.StartInfo = startInfo;
            try
            {
                process.Start();
                return true;
            }
            catch (Exception e)
            {
                LogUtils.LogException(e);
                return false;
            }
        }
    }
}
