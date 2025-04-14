using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a descriptor for an asset, providing access to its associated assembly
    /// and a method to retrieve the asset as a stream.
    /// </summary>
    public interface IAssetDescriptor
    {
        /// <summary>
        /// Gets the assembly associated with the asset.
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        /// Retrieves the asset as a stream.
        /// </summary>
        /// <returns>A <see cref="Stream"/> representing the asset.</returns>
        Stream GetStream();
    }
}
