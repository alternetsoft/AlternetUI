using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which allow to work with custom file system.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Gets whether the specified file exists.
        /// </summary>
        /// <param name="path">The file to check.</param>
        /// <returns>
        /// <c>true</c> if the caller has the required permissions and path contains the name of
        /// an existing file; otherwise, <c>false</c>. Also returns <c>false</c>
        /// if path is null, an invalid path, or a zero-length string.
        /// If the caller does not have sufficient
        /// permissions to read the specified file, no exception is thrown and the method
        /// returns <c>false</c> regardless of the existence of file.
        /// </returns>
        bool FileExists(string? path);

        /// <summary>
        /// Gets whether the given path refers to an existing directory.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns>
        /// <c>true</c> if path refers to an existing directory; <c>false</c> if the directory
        /// does not exist or an error occurs during the operation.
        /// </returns>
        bool DirectoryExists(string? path);

        /// <summary>
        /// Gets the names of subdirectories (including their paths) in the specified
        /// directory.
        /// </summary>
        /// <param name="path">The path (relative or absolute) to the directory to search.
        /// Value is not case-sensitive.</param>
        /// <returns>
        /// An array of the full names (with paths) of subdirectories in the specified
        /// path, or an empty array if no directories are found.
        /// </returns>
        string[] GetDirectories(string? path);

        /// <summary>
        /// Gets the names of files (with their paths) that match the specified search
        /// pattern in the specified directory.
        /// </summary>
        /// <param name="path">The path (relative or absolute) to the directory to search.
        /// Value is not case-sensitive.</param>
        /// <param name="searchPattern">
        /// The search string to match against the names of files in path. This parameter
        /// can contain a combination of valid literal path and wildcard (* and ?) characters.
        /// Regular expressions are not supported.
        /// </param>
        /// <returns>
        /// An array of the full names (with paths) for the files in the specified directory
        /// that match the search pattern, or an empty array if no files are found.
        /// </returns>
        string[] GetFiles(string? path, string? searchPattern);

        /// <summary>
        /// Opens an existing file for reading.
        /// </summary>
        /// <param name="path">The file to be opened for reading.</param>
        /// <returns>A read-only <see cref="Stream"/> on the specified path.</returns>
        Stream OpenRead(string path);

        /// <summary>
        /// Opens a <see cref="Stream"/> on the specified path with read/write
        /// access with no sharing.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">
        /// A <see cref="FileMode"/> value that specifies whether a file is created if one does
        /// not exist. Also determines whether the contents of existing file is retained
        /// or overwritten.
        /// </param>
        /// <returns>
        /// A <see cref="Stream"/> opened in the specified mode and path, with read/write
        /// access and not shared.
        /// </returns>
        Stream Open(string path, FileMode mode);

        /// <summary>
        /// Opens a <see cref="Stream"/> on the specified path, with the specified mode and
        /// access with no sharing.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">
        /// A <see cref="FileMode"/> value that specifies whether a file is created if one does
        /// not exist. Also determines whether the contents of existing file is retained
        /// or overwritten.
        /// </param>
        /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations
        /// that can be performed on the file.</param>
        /// <returns>
        /// A <see cref="Stream"/> on the specified path, having the specified parameters.
        /// </returns>
        Stream Open(string path, FileMode mode, FileAccess access);

        /// <summary>
        ///  Opens a <see cref="Stream"/> on the specified path, having the specified mode
        ///  with read, write, or read/write access and the specified sharing option.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <param name="mode">
        /// A <see cref="FileMode"/> value that specifies whether a file is created if one does
        /// not exist. Also determines whether the contents of existing file is retained
        /// or overwritten.
        /// </param>
        /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations
        /// that can be performed on the file.</param>
        /// <returns></returns>
        /// <param name="share">
        /// A <see cref="FileShare"/> value specifying the type of access other threads have
        /// to the file.
        /// </param>
        /// <returns>
        /// A <see cref="Stream"/> on the specified path, having the specified parameters.
        /// </returns>
        Stream Open(string path, FileMode mode, FileAccess access, FileShare share);

        /*/// <summary>
        /// Initializes a new instance of the <see cref="Stream"/> class with the specified
        /// path, creation mode, read/write and sharing permission, the access other streams
        /// can have to the same file, the buffer size, additional options and the allocation
        /// size.
        /// </summary>
        /// <param name="path">The path of the file to open.</param>
        /// <param name="options">An object that describes optional parameters to use.</param>
        /// <returns>A <see cref="Stream"/> instance that wraps the opened file.</returns>
        Stream Open(string path, FileStreamOptions options);*/

        /// <summary>
        /// Creates or overwrites a file in the specified path.
        /// </summary>
        /// <param name="path">The path and name of the file to create.</param>
        /// <returns>
        /// A <see cref="Stream"/> that provides read/write access to the file specified
        /// in path.
        /// </returns>
        Stream Create(string path);

        /// <summary>
        /// Creates or overwrites a file in the specified path, specifying a buffer size
        /// and options that describe how to create or overwrite the file.
        /// </summary>
        /// <param name="path">The path and name of the file to create.</param>
        /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
        /// <param name="options">
        /// One of the <see cref="FileOptions"/> values that describes how to create or overwrite
        /// the file.
        /// </param>
        /// <returns>A new file with the specified buffer size.</returns>
        Stream Create(string path, int bufferSize, FileOptions options);

        /// <summary>
        /// Creates or overwrites a file in the specified path, specifying a buffer size.
        /// </summary>
        /// <param name="path">The path and name of the file to create.</param>
        /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
        /// <returns>A new file with the specified buffer size.</returns>
        Stream Create(string path, int bufferSize);
    }
}
