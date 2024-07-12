using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines methods and properties which allow to perform different operations
    /// with image containers.
    /// </summary>
    public interface IImageContainer : IDisposable
    {
        /// <summary>
        /// Gets whether this object is dummy and does nothing.
        /// </summary>
        bool IsDummy { get; }

        /// <summary>
        /// Gets whether this object is ok.
        /// </summary>
        bool IsOk { get; }

        /// <summary>
        /// Gets whether this object is readonly.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Adds image to the container.
        /// </summary>
        /// <param name="image">Image to add.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        bool Add(Image image);

        /// <summary>
        /// Removes image with the specified index from the container.
        /// </summary>
        /// <param name="imageIndex">Image index.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        bool Remove(int imageIndex);

        /// <summary>
        /// Removes all images from the container.
        /// </summary>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        bool Clear();
    }
}
