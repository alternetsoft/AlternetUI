using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Image container bound to the dummy handler.
    /// </summary>
    public class ImageContainer : ImageContainer<DummyImageContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageContainer"/> class.
        /// </summary>
        public ImageContainer()
            : base(false)
        {
        }

        /// <inheritdoc/>
        protected override DummyImageContainer CreateHandler()
        {
            return new DummyImageContainer();
        }
    }
}
