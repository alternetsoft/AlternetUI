using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods which allow to work with the file system.
    /// All calls are translated to the <see cref="Default"/> file system.
    /// </summary>
    public static class FileSystem
    {
        /// <summary>
        /// Gets or sets default provider for the <see cref="IFileSystem"/>.
        /// All call to the static methods of the <see cref="FileSystem"/> are translated
        /// to this field.
        /// </summary>
        public static IFileSystem Default = new DefaultFileSystem();

        /// <inheritdoc cref="IFileSystem.FileExists"/>
        public static bool FileExists(string? path)
        {
            return Default.FileExists(path);
        }

        /// <inheritdoc cref="IFileSystem.DirectoryExists"/>
        public static bool DirectoryExists(string? path)
        {
            return Default.DirectoryExists(path);
        }

        /// <inheritdoc cref="IFileSystem.GetDirectories"/>
        public static string[] GetDirectories(string? path)
        {
            return Default.GetDirectories(path);
        }

        /// <inheritdoc cref="IFileSystem.GetFiles"/>
        public static string[] GetFiles(string? path, string? searchPattern)
        {
            return Default.GetFiles(path, searchPattern);
        }

        /// <inheritdoc cref="IFileSystem.OpenRead"/>
        public static Stream OpenRead(string path)
        {
            return Default.OpenRead(path);
        }

        /// <inheritdoc cref="IFileSystem.Open(string, FileMode)"/>
        public static Stream Open(string path, FileMode mode)
        {
            return Default.Open(path, mode);
        }

        /// <inheritdoc cref="IFileSystem.Open(string, FileMode, FileAccess)"/>
        public static Stream Open(string path, FileMode mode, FileAccess access)
        {
            return Default.Open(path, mode, access);
        }

        /// <inheritdoc cref="IFileSystem.Open(string, FileMode, FileAccess, FileShare)"/>
        public static Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return Default.Open(path, mode, access, share);
        }

        /// <inheritdoc cref="IFileSystem.Create(string)"/>
        public static Stream Create(string path)
        {
            return Default.Create(path);
        }

        /// <inheritdoc cref="IFileSystem.Create(string, int, FileOptions)"/>
        public static Stream Create(string path, int bufferSize, FileOptions options)
        {
            return Default.Create(path, bufferSize, options);
        }

        /// <inheritdoc cref="IFileSystem.Create(string, int)"/>
        public static Stream Create(string path, int bufferSize)
        {
            return Default.Create(path, bufferSize);
        }

        /*/// <inheritdoc cref="IFileSystem.Open(string, FileStreamOptions)"/>
        public static Stream Open(string path, FileStreamOptions options)
        {
            return Default.Open(path, options);
        }*/
    }
}