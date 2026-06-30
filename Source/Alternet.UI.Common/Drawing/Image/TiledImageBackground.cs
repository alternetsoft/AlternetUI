using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a tiled image background rendering mechanism.
    /// </summary>
    public class TiledImageBackground : DisposableObject
    {
        private Image? backgroundImage;
        private bool imageLoaded;

        /// <summary>
        /// Gets or sets the scale factor to apply when rendering the image.
        /// The default value is 1.0f (no scaling).
        /// </summary>
        public float ScaleFactor { get; set; } = 1.0f;

        /// <summary>
        /// Gets the source image used for rendering the background.
        /// </summary>
        public Image? SourceImage { get; internal set; }

        /// <summary>
        /// Gets the layout style used for the background image.
        /// </summary>
        public ImageLayout Layout { get; internal set; }

        /// <summary>
        /// Gets the rectangle representing the container area for the background image.
        /// </summary>
        public Rectangle ContainerRect { get; internal set; }

        /// <summary>
        /// Gets the rendered background image based on the source image, layout, and container rectangle.
        /// </summary>
        public virtual Image? BackgroundImage
        {
            get
            {
                if (!imageLoaded)
                {
                    try
                    {
                        backgroundImage = DrawingUtils.RenderBackgroundImage(
                            SourceImage,
                            Layout,
                            ContainerRect,
                            ScaleFactor);
                    }
                    catch (Exception ex)
                    {
                        Nop(ex);
                    }

                    imageLoaded = true;
                }

                return backgroundImage;
            }
        }

        /// <summary>
        /// Sets the source image, layout, and container rectangle for the background, and resets the rendered image.
        /// </summary>
        /// <param name="image">The source image to use.</param>
        /// <param name="layout">The layout style for the image.</param>
        /// <param name="clientRect">The rectangle representing the container area.</param>
        public virtual void SetImage(Image image, ImageLayout layout, Rectangle clientRect)
        {
            if (image != SourceImage || layout != Layout || clientRect != ContainerRect)
            {
                SourceImage = image;
                Layout = layout;
                ContainerRect = clientRect;
                SafeDispose(ref backgroundImage);
                imageLoaded = false;
            }
        }

        /// <summary>
        /// Releases managed resources used by the <see cref="TiledImageBackground"/>.
        /// </summary>
        protected override void DisposeManaged()
        {
            SafeDispose(ref backgroundImage);
            base.DisposeManaged();
        }
    }
}
