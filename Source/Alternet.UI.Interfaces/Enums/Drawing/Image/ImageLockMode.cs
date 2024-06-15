using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies flags that are passed to the flags parameter of
    /// the <see cref="Image.LockBits" /> method. The <see cref="Image.LockBits" /> method locks
    /// a portion of an image so that you can read or write the pixel data.
    /// </summary>
    public enum ImageLockMode
    {
        /// <summary>
        /// Specifies that a portion of the image is locked for reading.
        /// </summary>
        ReadOnly = 1,
        
        /// <summary>
        /// Specifies that a portion of the image is locked for writing.
        /// </summary>
        WriteOnly = 2,
        
        /// <summary>
        /// Specifies that a portion of the image is locked for reading or writing.
        /// </summary>
        ReadWrite = 3,
    }
}
