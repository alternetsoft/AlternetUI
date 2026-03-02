using System;
using System.IO;
using SkiaSharp;

using Alternet.UI;
using System.Collections.Generic;

namespace Alternet.Drawing;

/// <summary>
/// Represents a single frame within an animated image, including its bitmap data, display duration, and compositing
/// information.
/// </summary>
/// <remarks>This class provides access to both the original and fully composited bitmap representations of an
/// animation frame, as well as metadata such as frame index, display duration, and alpha channel type. It is typically
/// used when processing or rendering animated images, such as GIFs, where each frame may require compositing with
/// previous frames for correct display.</remarks>
public class AnimatedImageFrameInfo : DisposableObject
{
    private SKBitmap? originalBitmap;
    private SKBitmap? combinedBitmap;
    private Image? image;
    private List<ScaledImageData>? scaledImages;

    /// <summary>
    /// Gets or sets the original bitmap data as stored in the GIF frame. The bitmap may represent a partial or 'dirty'
    /// rectangle of the full image.
    /// </summary>
    /// <remarks>This property provides access to the unmodified bitmap data extracted from the GIF. The
    /// bitmap may not cover the entire image area if the frame only updates a portion of the image. Use this property
    /// when you need the exact pixel data as it appears in the GIF source, including any partial updates.</remarks>
    public SKBitmap? OriginalBitmap
    {
        get => originalBitmap;
        set => originalBitmap = value;
    }

    /// <summary>
    /// Gets or sets the fully composited bitmap that includes all required animation frames.
    /// </summary>
    public SKBitmap? CombinedBitmap
    {
        get => combinedBitmap;

        set
        {
            image = null;
            combinedBitmap = value;
        }
    }

    /// <summary>
    /// Gets the image representation associated with this frame, if available.
    /// </summary>
    /// <remarks>The image is initialized on first access from the value of the CombinedBitmap property, if it
    /// has not already been set. This property may return null if neither the image nor the CombinedBitmap is
    /// available.</remarks>
    public Image? Image
    {
        get
        {
            if (image == null)
            {
                if (CombinedBitmap != null)
                    image = (Image)CombinedBitmap;
            }

            return image;
        }
    }

    /// <summary>
    /// Gets or sets the index of the frame within the animated image sequence.
    /// </summary>
    public int Index { get; set; }
    
    /// <summary>
    /// Gets or sets the duration for which the frame should be displayed, in milliseconds.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the required frame number for processing.
    /// </summary>
    /// <remarks>This property specifies the frame that must be present for the operation to proceed. Ensure
    /// that the value is within the valid range of frame numbers applicable to the context.</remarks>
    public int RequiredFrame { get; set; }

    /// <summary>
    /// Gets or sets the alpha type used to define how pixel transparency is interpreted for this image frame.
    /// </summary>
    /// <remarks>The alpha type determines how the alpha channel is handled during rendering and blending
    /// operations. Common values include Premul (premultiplied alpha), Unpremul (unpremultiplied alpha), and Opaque.
    /// Setting the correct alpha type is important for accurate color blending and transparency effects.</remarks>
    public SKAlphaType AlphaType { get; set; }

    /// <summary>
    /// Returns an image scaled by the specified scale factor.
    /// </summary>
    /// <remarks>If the scale factor is approximately 1.0, the original image is returned without
    /// modification. Scaled images are cached to improve performance for repeated requests with the same scale factor.</remarks>
    /// <param name="scaleFactor">The factor by which to scale the image. A value of 1.0 returns the original image. Must be greater than 0.</param>
    /// <returns>A new image scaled by the specified factor, or null if the original image is not set.</returns>
    public virtual Image? GetScaledImage(float scaleFactor)
    {
        if (Image is null)
            return null;

        if (MathUtils.AreClose(scaleFactor, 1.0f))
            return Image;

        scaledImages ??= new ();

        for (int i = 0; i < scaledImages.Count; i++)
        {
            if (MathUtils.AreClose(scaledImages[i].ScaleFactor, scaleFactor))
                return scaledImages[i].ScaledImage;
        }

        var newSize = SizeI.Multiply(Image.Size, scaleFactor).ToSize();

        var scaledImage = new Bitmap(Image, newSize);

        scaledImages.Add(new ScaledImageData(scaleFactor, scaledImage));

        return scaledImage;
    }

    /// <inheritdoc/>
    protected override void DisposeManaged()
    {
        if (scaledImages is not null)
        {
            foreach (var scaledImageData in scaledImages)
            {
                scaledImageData.Dispose();
            }

            scaledImages.Clear();
            scaledImages = null;
        }

        SafeDispose(ref originalBitmap);
        SafeDispose(ref combinedBitmap);
        SafeDispose(ref image);
        base.DisposeManaged();
    }

    private class ScaledImageData : DisposableObject
    {
        private Image? scaledImage;

        public ScaledImageData(float scaleFactor, Image image)
        {
            ScaleFactor = scaleFactor;
            ScaledImage = image;
        }

        public float ScaleFactor { get; set; }

        public Image? ScaledImage { get => scaledImage; set => scaledImage = value; }

        protected override void DisposeManaged()
        {
            SafeDispose(ref scaledImage);
            base.DisposeManaged();
        }
    }
}

