using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements default provider for the <see cref="IFileSystem"/>.
    /// </summary>
    public class DefaultFileSystem : IFileSystem
    {
        /// <inheritdoc/>
        public virtual bool FileExists(string? path)
        {
            return File.Exists(path);
        }

        /// <inheritdoc/>
        public virtual bool DirectoryExists(string? path)
        {
            return Directory.Exists(path);
        }

        /// <inheritdoc/>
        public virtual string[] GetDirectories(string? path)
        {
            if (path is null)
                return Array.Empty<string>();
            return Directory.GetDirectories(path);
        }

        /// <inheritdoc/>
        public virtual string[] GetFiles(string? path, string? searchPattern)
        {
            if (path is null)
                return Array.Empty<string>();
            searchPattern ??= "*";
            return Directory.GetFiles(path, searchPattern);
        }

        /// <inheritdoc/>
        public virtual Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }
    }
}
