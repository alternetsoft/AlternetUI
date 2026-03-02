using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing;

/// <summary>
/// Represents a collection of animated images that can be displayed at various scale factors.
/// </summary>
/// <remarks>The AnimatedImageSet class provides methods to add, remove, and retrieve animated images based on
/// specified scale factors. It manages the lifecycle of the images, ensuring proper disposal of resources when images
/// are removed or the collection is cleared. This class is useful for scenarios where different image resolutions or
/// scale factors are required for display, such as supporting high-DPI environments.</remarks>
public partial class AnimatedImageSet : DisposableObject
{
    private readonly List<ImageItem> images = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimatedImageSet"/> class.
    /// </summary>
    public AnimatedImageSet()
    {
    }

    /// <summary>
    /// Gets the collection of image items available for display.
    /// </summary>
    /// <remarks>This property provides read-only access to the list of images. The collection is immutable,
    /// meaning that it cannot be modified directly through this property.</remarks>
    public IReadOnlyList<ImageItem> Images => images;

    /// <summary>
    /// Retrieves the animated image item that best matches the specified scale factor.
    /// </summary>
    /// <remarks>This method compares the provided scale factor to the scale factors of available images using
    /// a precision check. Ensure that the scale factor is within a valid range to retrieve the expected
    /// image item.</remarks>
    /// <param name="scaleFactor">The scale factor to match against available images. Must be a positive value.</param>
    /// <returns>An instance of AnimatedImage that corresponds to the specified scale factor,
    /// or null if no matching image item is found.</returns>
    public virtual ImageItem? GetItem(float scaleFactor)
    {
        ImageItem? bestMatch = null;
        float smallestDifference = float.MaxValue;

        foreach (var item in images)
        {
            if (MathUtils.AreClose(item.ScaleFactor, scaleFactor))
                return item;

            var newDifference = Math.Abs(item.ScaleFactor - scaleFactor);

            if (newDifference < smallestDifference)
            {
                smallestDifference = newDifference;
                bestMatch = item;
            }
        }

        return bestMatch;
    }

    /// <summary>
    /// Retrieves an animated image that is scaled by the specified factor.
    /// </summary>
    /// <param name="scaleFactor">The factor by which to scale the image. Must be a positive value.</param>
    /// <returns>An instance of <see cref="AnimatedImage"/> representing the scaled image, or <see langword="null"/> if no image
    /// is available for the specified scale factor.</returns>
    public virtual AnimatedImage? GetImage(float scaleFactor)
    {
        var item = GetItem(scaleFactor);
        return item?.Image;
    }

    /// <summary>
    /// Removes the image at the specified index from the collection and disposes of it if needed.
    /// </summary>
    /// <remarks>This method will not throw an exception for an invalid index but will return false instead.
    /// It is important to ensure that the index is valid before calling this method to avoid unexpected
    /// behavior.</remarks>
    /// <param name="index">The zero-based index of the image to remove. Must be within
    /// the bounds of the collection.</param>
    /// <returns>true if the image was successfully removed; otherwise, false.</returns>
    public virtual bool RemoveAt(int index)
    {
        if (index < 0 || index >= images.Count)
            return false;
        var image = images[index];
        images.RemoveAt(index);
        image.Dispose();
        return true;
    }

    /// <summary>
    /// Removes all animated images from the collection and releases any resources associated with them.
    /// </summary>
    public virtual void Clear()
    {
        foreach (var item in images)
            item.Dispose();
        images.Clear();
    }

    /// <summary>
    /// Adds the specified animated image to the collection.
    /// </summary>
    /// <param name="image">The animated image to add to the collection. This parameter cannot be null.</param>
    /// <param name="scaleFactor">The scale factor for which the animated image should be displayed.</param>
    /// <param name="disposeImage">A value indicating whether the image should be disposed of automatically after use.</param>
    /// <returns>true if the image was successfully added; otherwise, false.</returns>
    public virtual bool Add(AnimatedImage image, float scaleFactor, bool disposeImage = false)
    {
        if (image == null)
            return false;
        
        images.Add(new ImageItem(image, scaleFactor, disposeImage));
        return true;
    }

    /// <inheritdoc/>
    protected override void DisposeManaged()
    {
        Clear();
        base.DisposeManaged();
    }

    /// <summary>
    /// Represents an item that contains an animated image and its associated display scale factor.
    /// </summary>
    public class ImageItem : DisposableObject
    {
        /// <summary>
        /// Initializes a new instance of the ImageItem class using the specified animated image and scale
        /// factor.
        /// </summary>
        /// <remarks>The scale factor determines the size at which the animated image is displayed. Choose
        /// a value appropriate for the intended display context.</remarks>
        /// <param name="image">The animated image to associate with this item. This parameter cannot be null.</param>
        /// <param name="scaleFactor">The scale factor to apply to the animated image. Must be a positive value.</param>
        /// <param name="disposeImage">A value indicating whether the associated image should be disposed of automatically after use.</param>
        public ImageItem(AnimatedImage image, float scaleFactor, bool disposeImage = false)
        {
            Image = image;
            ScaleFactor = scaleFactor;
            IsImageDisposed = disposeImage;
        }
        
        /// <summary>
        /// Gets the animated image associated with this instance.
        /// </summary>
        /// <remarks>This property provides access to the current animated image, which can be used for
        /// rendering or display purposes. The animated image may contain multiple frames that can be played in
        /// sequence.</remarks>
        public AnimatedImage Image { get; }

        /// <summary>
        /// Gets the scale factor for the animated image.
        /// </summary>
        public float ScaleFactor { get; }

        /// <summary>
        /// Gets a value indicating whether the associated image is disposed of automatically after use.
        /// </summary>
        public bool IsImageDisposed { get; }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if (IsImageDisposed)
                Image.Dispose();
            base.DisposeManaged();
        }
    }
}

