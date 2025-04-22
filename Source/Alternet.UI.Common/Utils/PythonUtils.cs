using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to Python.
    /// </summary>
    public static class PythonUtils
    {
        /*
         On Ubuntu:
         LIBDIR: /usr/lib/x86_64-linux-gnu
         LIBRARY: libpython3.11.a
         LIBDEST: /usr/lib/python3.11

         On Windows:
         LIBDEST: C:\Users\UserName\AppData\Local\Programs\Python\Python312\Lib
        */

        /// <summary>
        /// The name of the Python configuration variable.
        /// </summary>
        public const string VarLibDir = "LIBDIR";

        /// <summary>
        /// The name of the Python configuration variable.
        /// </summary>
        public const string VarLibDest = "LIBDEST";

        private static string? pythonName;

        /// <summary>
        /// Gets or sets Python name. By default returns 'python' on Windows and
        /// 'python3' on other operating systems.
        /// </summary>
        /// <returns></returns>
        public static string PythonName
        {
            get
            {
                if (pythonName is not null)
                    return pythonName;

                string python;

                if (App.IsWindowsOS)
                    python = "python";
                else
                    python = "python3";

                return python;
            }

            set
            {
                pythonName = value;
            }
        }

        /// <summary>
        /// Executes a Python script using the application-defined method for running external commands.
        /// This method executes 'python' command using os command interpreter.
        /// </summary>
        /// <param name="script">The Python script to execute as a string.</param>
        /// <returns>
        /// The output of the Python script as a string, or <c>null</c>
        /// if an error occurs during execution.
        /// </returns>
        public static string? RunPythonScriptViaApp(string script)
        {
            string python = PythonName;

            try
            {
                var result = AppUtils.ExecuteApp(python, " -c \"" + script + "\"", null, true, false);
                return result.Output;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Executes 'python' command using os command interpreter and
        /// gets the config variable specified by it's name.
        /// </summary>
        /// <param name="s">Name of the config variable.</param>
        /// <returns></returns>
        public static string? GetPythonConfigVar(string s)
        {
            const string None = "None";

            const string script = "import sysconfig; print(sysconfig.get_config_var('{0}'));";
            var script1 = string.Format(script, s);

            var result = RunPythonScriptViaApp(script1);
            if (result == None)
                return null;
            return result;
        }

        /// <summary>
        /// Gets python library name prefix. Returns 'Python' on Windows
        /// and 'libpython' on other operating systems.
        /// </summary>
        /// <returns></returns>
        public static string GetPythonLibraryPrefix()
        {
            if (App.IsWindowsOS)
                return "Python";
            return "libpython";
        }

        /// <summary>
        /// Searches for the python library in the specified root folder.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="opt">Search options. Optional.
        /// Uses <see cref="SearchOption.TopDirectoryOnly"/> if not specified.</param>
        /// <returns></returns>
        public static string? FindPythonDll(
            string path,
            SearchOption opt = SearchOption.TopDirectoryOnly)
        {
            if (path != null && Directory.Exists(path))
            {
                string pythonMask =
                    GetPythonLibraryPrefix() + "*" + OSUtils.GetLibraryExtension();
                var result =
                   Directory.GetFiles(path, pythonMask, opt).OrderByDescending(x => x.Length)
                   .FirstOrDefault();
                return result;
            }
            else
                return null;
        }

        /// <summary>
        /// Gets python path using LIBDIR and LIBDEST python config vars.
        /// This is the correct way to find python on macOs or Linux.
        /// </summary>
        /// <returns></returns>
        internal static string? FindPythonPathUsingConfigVars()
        {
            var logLibVars = DebugUtils.IsDebugDefined;

            try
            {
                var result = Internal();
                return result;
            }
            catch (Exception e)
            {
                App.LogError(e);
                return null;
            }

            string? Internal()
            {
                var result1 = PythonUtils.GetPythonConfigVar(VarLibDir);

                App.DebugLogIf($"{VarLibDir}: {result1}", logLibVars);

                if (result1 is not null)
                {
                    var dll1 = FindPythonDll(result1, SearchOption.TopDirectoryOnly);
                    if (dll1 is not null)
                        return Path.GetDirectoryName(dll1);
                }

                var result3 = PythonUtils.GetPythonConfigVar(VarLibDest);
                App.LogIf($"{VarLibDest}: {result3}", logLibVars);

                if (result3 is not null)
                {
                    var dll3 = FindPythonDll(result3, SearchOption.AllDirectories);
                    if (dll3 is not null)
                        return Path.GetDirectoryName(dll3);
                }

                if (result3 is not null)
                {
                    var upperFolder = Path.Combine(result3, "..") + Path.DirectorySeparatorChar;
                    upperFolder = Path.GetFullPath(upperFolder);
                    var dll4 = FindPythonDll(upperFolder, SearchOption.AllDirectories);
                    if (dll4 is not null)
                        return Path.GetDirectoryName(dll4);
                }

                return null;
            }
        }
    }
}
