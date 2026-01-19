using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Alternet.UI
{
    /// <summary>
    /// Reads Windows version resource information from a native DLL/EXE.
    /// Returns a dictionary with standard version keys (FileVersion, ProductVersion, CompanyName, ProductName, etc.)
    /// and numeric parts (FileMajorPart, FileMinorPart, FileBuildPart, FilePrivatePart).
    /// </summary>
    public static class MswNativeVersionInfo
    {
        /// <summary>
        /// Logs version information for all files matching the specified name in the given directory.
        /// </summary>
        /// <param name="baseFolder">The path to the directory in which to search for files. Cannot be null or empty.</param>
        /// <param name="fileNameWithoutPath">The name of the file to search for, without any directory path. Cannot be null or empty.</param>
        /// <param name="recursive">true to search all subdirectories of the base folder; otherwise, false to search only the top-level
        /// directory.</param>
        /// <exception cref="ArgumentNullException">Thrown if baseFolder or fileNameWithoutPath is null or empty.</exception>
        public static void LogVersionInfo(string baseFolder, string fileNameWithoutPath, bool recursive)
        {
            if (string.IsNullOrEmpty(baseFolder))
                throw new ArgumentNullException(nameof(baseFolder));
            if (string.IsNullOrEmpty(fileNameWithoutPath))
                throw new ArgumentNullException(nameof(fileNameWithoutPath));
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var files = Directory.GetFiles(baseFolder, fileNameWithoutPath, searchOption);
            foreach (var file in files)
            {
                LogVersionInfo(file);
            }
        }

        /// <summary>
        /// Logs version information for the specified file to the application's logging system.
        /// </summary>
        /// <param name="filePath">The path to the file for which version information
        /// will be retrieved and logged. Cannot be null or empty.</param>
        public static void LogVersionInfo(string filePath)
        {
            var info = GetVersionInfo(filePath);
            LogUtils.LogDictionary("Version info for " + filePath, info);
        }

        /// <summary>
        /// Retrieves version information from the specified file as a dictionary of key-value pairs.
        /// </summary>
        /// <remarks>The returned dictionary may include fields such as "FileVersion", "ProductVersion",
        /// "CompanyName", and others, depending on the information available in the file's version resources. If the
        /// file does not contain version information or an error occurs during retrieval, the dictionary will be empty.
        /// This method uses both managed and native Windows APIs to extract as much version information as
        /// possible.</remarks>
        /// <param name="filePath">The path to the file from which to extract version information. Must not be null or empty.</param>
        /// <returns>A dictionary containing version information fields and their values. The dictionary is empty if the file
        /// does not exist or if version information cannot be retrieved.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filePath"/> is null or empty.</exception>
        /// <exception cref="PlatformNotSupportedException">Thrown if the current operating system is not Windows.</exception>
        public static IDictionary<string, string> GetVersionInfo(string filePath)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException("This API uses Windows version resource APIs.");

            if (!File.Exists(filePath))
                return result;

            // First, populate with FileVersionInfo (convenient)
            try
            {
                var fvi = FileVersionInfo.GetVersionInfo(filePath);

                AddIfNotNull(result, "FileVersion", fvi.FileVersion);
                AddIfNotNull(result, "ProductVersion", fvi.ProductVersion);
                AddIfNotNull(result, "CompanyName", fvi.CompanyName);
                AddIfNotNull(result, "ProductName", fvi.ProductName);
                AddIfNotNull(result, "FileDescription", fvi.FileDescription);
                AddIfNotNull(result, "InternalName", fvi.InternalName);
                AddIfNotNull(result, "OriginalFilename", fvi.OriginalFilename);
                AddIfNotNull(result, "Comments", fvi.Comments);
                AddIfNotNull(result, "LegalCopyright", fvi.LegalCopyright);
                AddIfNotNull(result, "LegalTrademarks", fvi.LegalTrademarks);
                AddIfNotNull(result, "PrivateBuild", fvi.PrivateBuild);
                AddIfNotNull(result, "SpecialBuild", fvi.SpecialBuild);

                // Numeric parts (FileVersion)
                result["FileMajorPart"] = fvi.FileMajorPart.ToString();
                result["FileMinorPart"] = fvi.FileMinorPart.ToString();
                result["FileBuildPart"] = fvi.FileBuildPart.ToString();
                result["FilePrivatePart"] = fvi.FilePrivatePart.ToString();
            }
            catch
            {
                // ignore - we'll still try the low-level API below
            }

            // Now try low-level Windows Version APIs to get localized string table and fixed file info.
            try
            {
                int handle;
                int size = GetFileVersionInfoSize(filePath, out handle);
                if (size > 0)
                {
                    var buffer = new byte[size];
                    if (GetFileVersionInfo(filePath, 0, size, buffer))
                    {
                        var gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        IntPtr pBlock = gch.AddrOfPinnedObject();
                        try
                        {
                            // Try to read VS_FIXEDFILEINFO at root ("\\")
                            if (VerQueryValue(pBlock, "\\", out IntPtr fixedPtr, out uint fixedLen) && fixedLen >= Marshal.SizeOf<VS_FIXEDFILEINFO>())
                            {
                                var ffi = Marshal.PtrToStructure<VS_FIXEDFILEINFO>(fixedPtr);
                                // Extract numeric version from ffi
                                uint fileVerMS = ffi.dwFileVersionMS;
                                uint fileVerLS = ffi.dwFileVersionLS;
                                uint major = (fileVerMS >> 16) & 0xffff;
                                uint minor = fileVerMS & 0xffff;
                                uint build = (fileVerLS >> 16) & 0xffff;
                                uint privatePart = fileVerLS & 0xffff;

                                result["FileVersionFromResource"] = $"{major}.{minor}.{build}.{privatePart}";
                                result["FileVersionMS"] = fileVerMS.ToString();
                                result["FileVersionLS"] = fileVerLS.ToString();
                            }

                            // Read translation table: language+codepage
                            if (VerQueryValue(pBlock, @"\VarFileInfo\Translation", out IntPtr transPtr, out uint transLen) && transLen >= 4)
                            {
                                // read first translation (WORD lang, WORD codepage)
                                ushort lang = (ushort)Marshal.ReadInt16(transPtr);
                                ushort codepage = (ushort)Marshal.ReadInt16(transPtr + 2);
                                string langCp = lang.ToString("x4") + codepage.ToString("x4"); // e.g. "040904b0"

                                // keys we commonly want
                                string[] keys = new[]
                                {
                                    "FileDescription", "FileVersion", "InternalName", "OriginalFilename",
                                    "ProductName", "ProductVersion", "Comments", "CompanyName",
                                    "LegalCopyright", "LegalTrademarks", "PrivateBuild", "SpecialBuild"
                                };

                                foreach (var key in keys)
                                {
                                    string subBlock = $@"\StringFileInfo\{langCp}\{key}";
                                    if (VerQueryValue(pBlock, subBlock, out IntPtr valPtr, out uint valLen) && valLen > 0)
                                    {
                                        string value = Marshal.PtrToStringUni(valPtr) ?? string.Empty;
                                        // prefer low-level read if FileVersionInfo was missing or empty
                                        if (!result.ContainsKey(key) || string.IsNullOrEmpty(result[key]))
                                            result[key] = value;
                                        else
                                        {
                                            // keep both: low-level value under key+" (lang)" if different
                                            if (!string.Equals(result[key], value, StringComparison.Ordinal))
                                                result[$"{key} ({langCp})"] = value;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Try a common fallback locale 040904b0 (en-US + Unicode)
                                string langCp = "040904b0";
                                string[] keys = new[]
                                {
                                    "FileDescription", "FileVersion", "InternalName", "OriginalFilename",
                                    "ProductName", "ProductVersion", "Comments", "CompanyName",
                                    "LegalCopyright", "LegalTrademarks", "PrivateBuild", "SpecialBuild"
                                };
                                foreach (var key in keys)
                                {
                                    string subBlock = $@"\StringFileInfo\{langCp}\{key}";
                                    if (VerQueryValue(pBlock, subBlock, out IntPtr valPtr, out uint valLen) && valLen > 0)
                                    {
                                        string value = Marshal.PtrToStringUni(valPtr) ?? string.Empty;
                                        if (!result.ContainsKey(key) || string.IsNullOrEmpty(result[key]))
                                            result[key] = value;
                                    }
                                }
                            }
                        }
                        finally
                        {
                            gch.Free();
                        }
                    }
                }
            }
            catch
            {
                // ignore errors from native calls; we already populated what FileVersionInfo provided
            }

            return result;
        }

        private static void AddIfNotNull(IDictionary<string, string> dict, string key, string? value)
        {
            if (!string.IsNullOrEmpty(value))
                dict[key] = value!;
        }

        #region Native version.dll interop

        [StructLayout(LayoutKind.Sequential)]
        private struct VS_FIXEDFILEINFO
        {
            public uint dwSignature;
            public uint dwStrucVersion;
            public uint dwFileVersionMS;
            public uint dwFileVersionLS;
            public uint dwProductVersionMS;
            public uint dwProductVersionLS;
            public uint dwFileFlagsMask;
            public uint dwFileFlags;
            public uint dwFileOS;
            public uint dwFileType;
            public uint dwFileSubtype;
            public uint dwFileDateMS;
            public uint dwFileDateLS;
        }

        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetFileVersionInfoSize(string lptstrFilename, out int lpdwHandle);

        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetFileVersionInfo(string lptstrFilename, int dwHandle, int dwLen, [In] byte[] lpData);

        // Note: we pin the buffer and pass the pointer to VerQueryValue
        [DllImport("version.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool VerQueryValue(IntPtr pBlock, string lpSubBlock, out IntPtr lplpBuffer, out uint puLen);

        #endregion
    }
}