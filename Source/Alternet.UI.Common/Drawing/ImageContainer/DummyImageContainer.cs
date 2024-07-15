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
    /// Dummy implementation of the <see cref="IImageContainer"/> interface.
    /// </summary>
    public class DummyImageContainer : DisposableObject, IImageContainer
    {
        /// <inheritdoc/>
        public virtual bool IsOk => true;

        /// <inheritdoc/>
        public virtual bool IsReadOnly => false;

        /// <inheritdoc/>
        public virtual bool IsDummy => true;

        /// <inheritdoc/>
        public virtual bool Add(Image image)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool Clear()
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual bool Remove(int imageIndex)
        {
            return true;
        }

        /// <summary>
        /// Loads image from stream and adds it to the container.
        /// </summary>
        /// <param name="stream">Stream with image data.</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool LoadFromStream(Stream stream)
        {
            return true;
        }
    }
}
