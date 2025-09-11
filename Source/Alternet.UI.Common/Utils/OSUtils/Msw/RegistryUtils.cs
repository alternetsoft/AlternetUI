using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to Windows registry.
    /// </summary>
    public static class RegistryUtils
    {
        /// <summary>
        /// Gets or sets whether to use <see cref="RegKeyWow6432Node"/> when
        /// <see cref="WriteRelativeValue"/> and other relative read/write operations are performed.
        /// Default is <c>false</c>.
        /// </summary>
        public static bool UseWow6432Node = false;

        /// <summary>
        /// Default reg key suffix for the Alternet.UI.
        /// </summary>
        public static string RegKeySuffix = "\\AlterNET Software\\Alternet.UI\\";

        /// <summary>
        /// Default reg key for the Alternet.UI.
        /// </summary>
        public static string RegKey = "Software" + RegKeySuffix;

        /// <summary>
        /// Default reg key for the Alternet.UI on Wow6432.
        /// </summary>
        public static string RegKeyWow6432Node = "Software\\WOW6432Node" + RegKeySuffix;

        /// <summary>
        /// Writes value to Windows registry.
        /// </summary>
        /// <param name="keyPath">Path to value.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="hive"><see cref="RegistryHive"/> value which specifies base path.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="valueKind">Value kind.</param>
        /// <returns></returns>
        public static bool WriteValue(
            string keyPath,
            string valueName,
            object value,
            RegistryValueKind valueKind,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            try
            {
                using var lockedKey = new RegistryKeyLock(hive, keyPath, true);
                if (lockedKey.SubKey is null)
                    return false;
                lockedKey.SubKey.SetValue(valueName, value, valueKind);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes value to Windows registry key specified
        /// with the relative path.
        /// </summary>
        /// <param name="value">Value to write.</param>
        /// <param name="valueKind">Value kind.</param>
        /// <param name="relativeKeyPath">Relative path to value.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="hive"><see cref="RegistryHive"/> value which specifies base path.</param>
        /// <returns></returns>
        public static bool WriteRelativeValue(
            string relativeKeyPath,
            string valueName,
            object value,
            RegistryValueKind valueKind,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            if (UseWow6432Node)
            {
                var result = WriteValue(RegKeyWow6432Node + relativeKeyPath, valueName, value, valueKind, hive);
                if (result)
                    return true;
            }

            return WriteValue(RegKey + relativeKeyPath, valueName, value, valueKind, hive);
        }

        /// <summary>
        /// Retrieves an array of strings that contains all the subkey names.
        /// </summary>
        /// <param name="keyPath">Path to value.</param>
        /// <param name="hive"><see cref="RegistryHive"/> value which specifies base path.</param>
        /// <returns>
        /// An array of strings that contains the names of the subkeys for the current key.
        /// </returns>
        public static string[]? GetSubKeyNames(
            string keyPath,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            try
            {
                using var lockedKey = new RegistryKeyLock(hive, keyPath);
                var subKeyNames = lockedKey.SubKey?.GetSubKeyNames();
                return subKeyNames;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves an array of strings that contains all the subkey names specified
        /// with the relative path to the registry key.
        /// </summary>
        /// <param name="relativeKeyPath">Relative path to value.</param>
        /// <param name="hive"><see cref="RegistryHive"/> value which specifies base path.</param>
        /// <returns>
        /// An array of strings that contains the names of the subkeys for the current key.
        /// </returns>
        public static string[]? GetRelativeSubKeyNames(
            string relativeKeyPath,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            if (UseWow6432Node)
            {
                var result = GetSubKeyNames(RegKeyWow6432Node + relativeKeyPath, hive);
                if (result != null)
                    return result;
            }

            var result2 = GetSubKeyNames(RegKey + relativeKeyPath, hive);
            return result2;
        }

        /// <summary>
        /// Reads string value from Windows registry key specified
        /// with the relative path.
        /// </summary>
        /// <param name="relativeKeyPath">Relative path to value.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="hive"><see cref="RegistryHive"/> value which specifies base path.</param>
        /// <returns></returns>
        public static string? ReadRelativeString(
            string relativeKeyPath,
            string valueName,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            if (UseWow6432Node)
            {
                var text = ReadString(RegKeyWow6432Node + relativeKeyPath, valueName, hive);
                if (text != null)
                    return text;
            }

            return ReadString(RegKey + relativeKeyPath, valueName, hive);
        }

        /// <summary>
        /// Reads string value from Windows registry.
        /// </summary>
        /// <param name="keyPath">Path to value.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="hive"><see cref="RegistryHive"/> value which specifies base path.</param>
        /// <returns></returns>
        public static string? ReadString(
            string keyPath,
            string valueName,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            return (string?)ReadValue(keyPath, valueName, hive);
        }

        /// <summary>
        /// Reads value from Windows registry key specified
        /// with the relative path.
        /// </summary>
        /// <param name="relativeKeyPath">Relative path to value.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="hive"><see cref="RegistryHive"/> value which specifies base path.</param>
        /// <returns></returns>
        public static object? ReadRelativeValue(
            string relativeKeyPath,
            string valueName,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            if (UseWow6432Node)
            {
                var text = ReadValue(RegKeyWow6432Node + relativeKeyPath, valueName, hive);
                if (text != null)
                    return text;
            }

            return ReadValue(RegKey + relativeKeyPath, valueName, hive);
        }

        /// <summary>
        /// Reads value from Windows registry.
        /// </summary>
        /// <param name="keyPath">Path to value.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="hive"><see cref="RegistryHive"/> value which specifies base path.</param>
        /// <returns></returns>
        public static object? ReadValue(
            string keyPath,
            string valueName,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            try
            {
                using var lockedKey = new RegistryKeyLock(hive, keyPath);
                var value = lockedKey.SubKey?.GetValue(valueName);
                return value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Writes to registry path to the UIXmlPreview application.
        /// </summary>
        /// <param name="path">Path to the UIXmlPreview application</param>
        /// <returns></returns>
        public static bool WriteUIXmlPreviewPath(string path)
        {
            return WriteRelativeValue("UIXmlPreview", "Path", path, RegistryValueKind.String, RegistryHive.CurrentUser);
        }

        /// <summary>
        /// Reads from registry path to the UIXmlPreview application.
        /// </summary>
        public static string? ReadUIXmlPreviewPath()
        {
            return ReadRelativeString("UIXmlPreview", "Path", RegistryHive.CurrentUser);
        }

        internal class RegistryKeyLock : IDisposable
        {
            public RegistryKeyLock(RegistryHive hive, string keyPath, bool create = false)
            {
                RegistryView view = Environment.Is64BitOperatingSystem
                    ? RegistryView.Registry64 : RegistryView.Registry32;
                BaseKey = RegistryKey.OpenBaseKey(hive, view);
                if (BaseKey == null)
                    return;
                if (create)
                    SubKey = BaseKey.CreateSubKey(keyPath, writable: true);
                else
                    SubKey = BaseKey.OpenSubKey(keyPath);
                if (SubKey == null)
                {
                    BaseKey.Close();
                    BaseKey = null;
                    return;
                }
            }

            public RegistryKey? BaseKey { get; set; }

            public RegistryKey? SubKey { get; set; }

            public void Dispose()
            {
                SubKey?.Close();
                SubKey = null;
                BaseKey?.Close();
                BaseKey = null;
            }
        }
    }
}
