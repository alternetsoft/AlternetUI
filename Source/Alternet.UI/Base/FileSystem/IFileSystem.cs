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
    }
}