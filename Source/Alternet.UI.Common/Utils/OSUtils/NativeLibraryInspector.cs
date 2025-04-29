using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Alternet.UI;

/// <summary>
/// Provides functionality to inspect native libraries without loading them.
/// Supports Windows, Linux, and macOS.
/// </summary>
public static class NativeLibraryInspector
{
    /// <summary>
    /// Retrieves a list of native libraries referenced by the currently running application.
    /// This method does not attempt to load the libraries.
    /// </summary>
    /// <returns>An array of strings containing the names of referenced native libraries.</returns>
    public static string[] GetCurrentApplicationReferences()
    {
        string? executablePath = Process.GetCurrentProcess().MainModule?.FileName;

        if (string.IsNullOrEmpty(executablePath) || !File.Exists(executablePath))
        {
            return [];
        }

        return GetReferencedNativeLibraries(executablePath);
    }

    /// <summary>
    /// Retrieves a list of native libraries referenced by the specified library file.
    /// This method does not load the library.
    /// </summary>
    /// <param name="libraryPath">The path to the native library file.</param>
    /// <returns>An array of referenced library names.</returns>
    public static string[] GetReferencedNativeLibraries(string? libraryPath)
    {
        if (string.IsNullOrEmpty(libraryPath) || !System.IO.File.Exists(libraryPath))
        {
            return [];
        }

        var command = GetDependencyCommand(libraryPath!);
        if (string.IsNullOrEmpty(command))
        {
            return [];
        }

        try
        {
            var output = AppUtils.ExecuteTerminalCommand(command!, null, true, false);

            if (output.Output is null)
                return [];

            var result = ParseLibraryNames(output.Output);
            return result;
        }
        catch
        {
            return [];
        }
    }

    /// <summary>
    /// Searches common Visual Studio paths to locate dumpbin.exe.
    /// </summary>
    /// <returns>The full path to dumpbin.exe if found; otherwise, null.</returns>
    private static string? FindDumpbin()
    {
        string[] possibleRoots =
        {
            @"C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Tools\MSVC",
            @"C:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Tools\MSVC",
            @"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\VC\Tools\MSVC",
            @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Tools\MSVC",
            @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\VC\Tools\MSVC",
            @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\VC\Tools\MSVC",
        };

        foreach (var root in possibleRoots)
        {
            if (Directory.Exists(root))
            {
                var msvcVersions = Directory.GetDirectories(root);
                foreach (var version in msvcVersions)
                {
                    string dumpbinPath = Path.Combine(version, "bin", "Hostx64", "x64", "dumpbin.exe");
                    if (File.Exists(dumpbinPath))
                    {
                        return dumpbinPath;
                    }
                }
            }
        }

        return null;
    }

    private static string? GetDependencyCommand(string libraryPath)
    {
        if (App.IsWindowsOS)
        {
            var pathToCommand = FindDumpbin();
            if (pathToCommand is null)
                return null;
            return $"\"{pathToCommand}\" /DEPENDENTS \"{libraryPath}\"";
        }
        else
        if (App.IsLinuxOS)
            return $"objdump -p \"{libraryPath}\" | grep NEEDED";
        else
        if (App.IsMacOS)
            return $"otool -L \"{libraryPath}\"";
        else
            return null;
    }

    private static string[] ParseLibraryNames(string output)
    {
        var libraries = new System.Collections.Generic.List<string>();
        foreach (var line in output.Split('\n'))
        {
            string trimmed = line.Trim();

            if (string.IsNullOrEmpty(trimmed))
                continue;

            if (App.IsWindowsOS)
            {
                if (trimmed.Contains(" ") || !trimmed.Contains("."))
                    continue;
            }

            libraries.Add(trimmed);
        }

        return libraries.ToArray();
    }
}
