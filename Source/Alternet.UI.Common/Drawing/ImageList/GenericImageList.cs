using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a generic image list that does not have a specific handler implementation.
    /// This image list is not bound to the native image list and can be attached to generic controls.
    /// If you need to use a native image list, you should use the <see cref="ImageList"/> class instead.
    /// </summary>
    /// <remarks><see cref="GenericImageList"/> works faster than <see cref="ImageList"/>
    /// because it does not have a native handler.</remarks>
    public partial class GenericImageList : ImageList
    {
        /// <inheritdoc/>
        protected override IImageListHandler? CreateHandler()
        {
            return null;
        }
    }
}
