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
        /// Default reg key suffix for the Alternet.UI.
        /// </summary>
        public static string RegKeySuffix = "\\Alternet\\Alternet.UI\\";

        /// <summary>
        /// Default reg key for the Alternet.UI.
        /// </summary>
        public static string RegKey = "SOFTWARE" + RegKeySuffix;

        /// <summary>
        /// Default reg key for the Alternet.UI on Wow6432.
        /// </summary>
        public static string RegKeyWow6432Node = "SOFTWARE\\WOW6432Node" + RegKeySuffix;

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
            using var lockedKey = new RegistryKeyLock(hive, keyPath);
            if (lockedKey.SubKey is null)
                return false;
            lockedKey.SubKey.SetValue(valueName, value, valueKind);
            return true;
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
        /// <remarks>
        /// This method calls <see cref="WriteValue"/> using <see cref="RegKeyWow6432Node"/>
        /// as a base key path with <paramref name="relativeKeyPath"/> as a subkey path.
        /// If call is not successful, <see cref="WriteValue"/> is called again with
        /// <see cref="RegKey"/> as a base key path with <paramref name="relativeKeyPath"/>
        /// as a subkey path.
        /// So this method allows transparent access to the registry making it independent from
        /// the bitness (64 bit or 32 bit) of the application and operating system.
        /// </remarks>
        public static bool WriteRelativeValue(
            string relativeKeyPath,
            string valueName,
            object value,
            RegistryValueKind valueKind,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            var result = WriteValue(RegKeyWow6432Node + relativeKeyPath, valueName, value, valueKind, hive);
            if (result)
                return true;
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
            using var lockedKey = new RegistryKeyLock(hive, keyPath);
            var subKeyNames = lockedKey.SubKey?.GetSubKeyNames();
            return subKeyNames;
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
        /// <remarks>
        /// This method calls <see cref="GetSubKeyNames"/> using <see cref="RegKeyWow6432Node"/>
        /// as a base key path with <paramref name="relativeKeyPath"/> as a subkey path.
        /// If call is not successful, <see cref="GetSubKeyNames"/> is called again with
        /// <see cref="RegKey"/> as a base key path with <paramref name="relativeKeyPath"/>
        /// as a subkey path.
        /// So this method allows transparent access to the registry making it independent from
        /// the bitness (64 bit or 32 bit) of the application and operating system.
        /// </remarks>
        public static string[]? GetRelativeSubKeyNames(
            string relativeKeyPath,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            var result = GetSubKeyNames(RegKeyWow6432Node + relativeKeyPath, hive);
            if (result != null)
                return result;
            result = GetSubKeyNames(RegKey + relativeKeyPath, hive);
            return result;
        }

        /// <summary>
        /// Reads string value from Windows registry key specified
        /// with the relative path.
        /// </summary>
        /// <param name="relativeKeyPath">Relative path to value.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="hive"><see cref="RegistryHive"/> value which specifies base path.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method calls <see cref="ReadString"/> using <see cref="RegKeyWow6432Node"/>
        /// as a base key path with <paramref name="relativeKeyPath"/> as a subkey path.
        /// If call is not successful, <see cref="ReadString"/> is called again with
        /// <see cref="RegKey"/> as a base key path with <paramref name="relativeKeyPath"/>
        /// as a subkey path.
        /// So this method allows transparent access to the registry making it independent from
        /// the bitness (64 bit or 32 bit) of the application and operating system.
        /// </remarks>
        public static string? ReadRelativeString(
            string relativeKeyPath,
            string valueName,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            var text = ReadString(RegKeyWow6432Node + relativeKeyPath, valueName, hive);
            if (text != null)
                return text;
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
        /// <remarks>
        /// This method calls <see cref="ReadValue"/> using <see cref="RegKeyWow6432Node"/>
        /// as a base key path with <paramref name="relativeKeyPath"/> as a subkey path.
        /// If call is not successful, <see cref="ReadValue"/> is called again with
        /// <see cref="RegKey"/> as a base key path with <paramref name="relativeKeyPath"/>
        /// as a subkey path.
        /// So this method allows transparent access to the registry making it independent from
        /// the bitness (64 bit or 32 bit) of the application and operating system.
        /// </remarks>
        public static object? ReadRelativeValue(
            string relativeKeyPath,
            string valueName,
            RegistryHive hive = RegistryHive.LocalMachine)
        {
            var text = ReadValue(RegKeyWow6432Node + relativeKeyPath, valueName, hive);
            if (text != null)
                return text;
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
            using var lockedKey = new RegistryKeyLock(hive, keyPath);
            var value = lockedKey.SubKey?.GetValue(valueName);
            return value;
        }

        internal class RegistryKeyLock : IDisposable
        {
            public RegistryKeyLock(RegistryHive hive, string keyPath)
            {
                RegistryView view = Environment.Is64BitOperatingSystem
                    ? RegistryView.Registry64 : RegistryView.Registry32;
                BaseKey = RegistryKey.OpenBaseKey(hive, view);
                if (BaseKey == null)
                    return;

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
