using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a dynamic bitmap that supports lazy initialization, scaling, and transparency.
    /// </summary>
    /// <remarks>This class provides a base implementation for managing a bitmap resource that can be
    /// dynamically resized, scaled, and configured for transparency.
    /// The bitmap is lazily created on first access and
    /// disposed of when the properties affecting its state are modified. Derived classes must implement
    /// the <see cref="CreateBitmap"/> method to define how the bitmap resource is created.</remarks>
    /// <typeparam name="T">The type of the underlying bitmap resource, which must implement
    /// <see cref="IDisposableObject"/>.</typeparam>
    public abstract class DynamicBitmap<T> : DisposableObject
        where T : class, IDisposableObject
    {
        private SizeD size;
        private Coord scaleFactor = 1;
        private bool isTransparent;
        private T? bitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicBitmap"/> class.
        /// </summary>
        public DynamicBitmap()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicBitmap"/> class.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="isTransparent"></param>
        public DynamicBitmap(SizeD size, Coord scaleFactor, bool isTransparent)
        {
            this.size = size;
            this.scaleFactor = scaleFactor;
            this.isTransparent = isTransparent;
        }

        /// <summary>
        /// Gets the bitmap, creating it if necessary.
        /// </summary>
        /// <remarks>The bitmap is lazily created on the first access.
        /// Subsequent accesses return the same instance.</remarks>
        public virtual T Bitmap
        {
            get
            {
                bitmap ??= CreateBitmap();
                return bitmap;
            }
        }

        /// <summary>
        /// Gets the size of the bitmap in pixels, adjusted for the <see cref="ScaleFactor"/>.
        /// </summary>
        public virtual SizeI SizeInPixels
        {
            get
            {
                return Size.PixelFromDip(ScaleFactor);
            }
        }

        /// <summary>
        /// Gets or sets the size of the bitmap canvas in device-independent units.
        /// </summary>
        public virtual SizeD Size
        {
            get => size;

            set
            {
                if (size == value)
                    return;
                size = value;
                DisposeBitmap();
            }
        }

        /// <summary>
        /// Gets or sets the scale factor used for the bitmap canvas.
        /// </summary>
        public virtual Coord ScaleFactor
        {
            get => scaleFactor;

            set
            {
                if (scaleFactor == value)
                    return;
                scaleFactor = value;
                DisposeBitmap();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the bitmap canvas is transparent.
        /// </summary>
        public virtual bool IsTransparent
        {
            get => isTransparent;

            set
            {
                if (isTransparent == value)
                    return;
                isTransparent = value;
                DisposeBitmap();
            }
        }

        /// <summary>
        /// Determines whether the specified size, scale factor,
        /// and transparency flag are equal to those used in the current instance.
        /// </summary>
        /// <param name="size">The size to compare with the value stored in the current instance.</param>
        /// <param name="scaleFactor">The scale factor to compare with the value
        /// stored in the current instance.</param>
        /// <param name="isTransparent">A value indicating whether the transparency flag to compare
        /// matches the value stored in the current instance.</param>
        /// <returns><see langword="true"/> if the specified <see cref="SizeD"/>, <see cref="Coord"/>,
        /// and transparency flag  are equal to the values stored in the current instance;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Equals(SizeD size, Coord scaleFactor, bool isTransparent)
        {
            return Size == size && ScaleFactor == scaleFactor && IsTransparent == isTransparent;
        }

        /// <summary>
        /// Creates and returns a new bitmap instance.
        /// </summary>
        /// <remarks>The specific type of bitmap created depends
        /// on the implementation of the derived class.</remarks>
        /// <returns>A new instance of type <typeparamref name="T"/> representing the created bitmap.</returns>
        public abstract T CreateBitmap();

        /// <summary>
        /// Updates the dynamic properties of the object, including size, scale factor, and transparency.
        /// </summary>
        /// <remarks>If the specified properties are the same as the current properties, the method
        /// performs no action. Otherwise, the method updates the properties and disposes of any associated
        /// resources as needed.</remarks>
        /// <param name="size">The new size of the object.</param>
        /// <param name="scaleFactor">The scaling factor to apply to the object.</param>
        /// <param name="isTransparent">A value indicating whether the object should be transparent.</param>
        public virtual void SetDynamicProperties(SizeD size, Coord scaleFactor, bool isTransparent)
        {
            if (Equals(size, scaleFactor, isTransparent))
                return;

            this.size = size;
            this.scaleFactor = scaleFactor;
            this.isTransparent = isTransparent;

            DisposeBitmap();
        }

        /// <summary>
        /// Releases the resources used by the bitmap object and sets it to null.
        /// </summary>
        /// <remarks>This method is intended to be called to explicitly release the bitmap resource.
        /// It ensures that the bitmap is disposed safely and prevents memory leaks.
        /// Override this method in a derived
        /// class to provide additional cleanup logic, but ensure the base implementation is called to dispose the
        /// bitmap properly.</remarks>
        protected virtual void DisposeBitmap()
        {
            SafeDisposeObject(ref bitmap);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            DisposeBitmap();
        }
    }
}
