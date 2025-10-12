using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Storage for <see cref="SvgImage"/> conversions to <see cref="Image"/>
    /// for the different visual states and light/dark flags.
    /// </summary>
    public struct CachedSvgImage<TImage>
        where TImage : class
    {
        private EnumArray<VisualControlState, LightDarkImage<object>?> imageData = new();
        private SvgImage? svgImage;
        private SizeI? svgSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedSvgImage{TImage}"/> struct.
        /// </summary>
        public CachedSvgImage()
        {
        }

        /// <summary>
        /// Gets or sets svg size.
        /// </summary>
        public SizeI? SvgSize
        {
            readonly get => svgSize;

            set
            {
                if (svgSize == value)
                    return;
                svgSize = value;
                imageData.Reset();
            }
        }

        /// <summary>
        /// Gets or sets svg.
        /// </summary>
        public SvgImage? SvgImage
        {
            readonly get
            {
                return svgImage;
            }

            set
            {
                if (svgImage == value)
                    return;
                svgImage = value;
                imageData.Reset();
            }
        }

        /// <summary>
        /// Resets the cached image data to its initial state.
        /// </summary>
        /// <remarks>This method clears any cached image data, ensuring that subsequent operations will
        /// use fresh data. If no image data is currently cached, the method has no effect.</remarks>
        public void ResetCachedImages()
        {
            if(svgImage != null)
                imageData.Reset();
        }

        /// <summary>
        /// Gets whether image exists for the specified item state and light/dark theme flag.
        /// </summary>
        /// <param name="state">Item state.</param>
        /// <param name="isDark">Light/dark theme flag.</param>
        /// <returns></returns>
        public readonly bool HasImage(VisualControlState state, bool? isDark = null)
        {
            return GetImage(state, isDark) != null;
        }

        /// <summary>
        /// Gets image for the specified item state and light/dark theme flag.
        /// </summary>
        /// <param name="state">Item state.</param>
        /// <param name="isDark">Light/dark theme flag.</param>
        /// <returns></returns>
        public readonly TImage? GetImage(VisualControlState state, bool? isDark = null)
        {
            isDark ??= LightDarkColor.IsUsingDarkColor;
            var result = imageData[state]?.GetImage(isDark.Value);
            return (TImage?)result;
        }

        /// <summary>
        /// Sets image for the specified color theme light/dark theme flag.
        /// </summary>
        /// <param name="state">Visual state (normal, disabled, selected)
        /// for which image is set.</param>
        /// <param name="image">New image value.</param>
        /// <param name="isDark">Whether theme is dark.</param>
        public void SetImage(VisualControlState state, object? image, bool? isDark = null)
        {
            isDark ??= LightDarkColor.IsUsingDarkColor;
            imageData[state] ??= new();
            imageData[state]!.SetImage(isDark.Value, image);
        }

        /// <summary>
        /// Gets real svg height in pixels using <see cref="SvgSize"/> and other settings.
        /// </summary>
        /// <param name="control">Control which scale factor is used.</param>
        /// <returns></returns>
        public readonly int RealSvgHeight(Control? control = null)
        {
            var imageSize = SvgSize ?? 16;
            var imageHeight = imageSize.Height;
            return imageHeight;
        }

        /// <summary>
        /// Saves svg as <see cref="ImageSet"/> to the specified state.
        /// </summary>
        /// <param name="state">Item state.</param>
        /// <param name="isDark">Light/dark theme flag.</param>
        /// <param name="control">Control which scale factor is used.</param>
        public void UpdateImageSet(VisualControlState state, bool isDark, Control? control)
        {
            if (svgImage is null)
                return;

            if (!HasImage(state, isDark))
            {
                SetImage(
                    state,
                    svgImage?.AsNormal(RealSvgHeight(control), isDark),
                    isDark);
            }
        }

        /// <summary>
        /// Assigns the values from the specified <see cref="CachedSvgImage{TImage}"/> instance to this instance.
        /// </summary>
        /// <param name="other">The <see cref="CachedSvgImage{TImage}"/> instance whose values
        /// will be assigned to this instance.</param>
        public void Assign(in CachedSvgImage<TImage> other)
        {
            svgImage = other.svgImage;
            svgSize = other.svgSize;
            imageData = other.imageData.Clone();
        }

        /// <summary>
        /// Creates a new instance of <see cref="CachedSvgImage{TImage}"/> that is a copy of the current instance.
        /// </summary>
        /// <remarks>The cloned instance will have the same state and data as the original instance.
        /// Changes made to the cloned instance will not affect the original instance, and vice versa.</remarks>
        /// <returns>A new <see cref="CachedSvgImage{TImage}"/> instance that is a copy of the current instance.</returns>
        public CachedSvgImage<TImage> Clone()
        {
            var result = new CachedSvgImage<TImage>();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Saves svg as <see cref="Image"/> to the specified state.
        /// </summary>
        /// <param name="state">Item state.</param>
        /// <param name="isDark">Light/dark theme flag.</param>
        /// <param name="control">Control which scale factor is used.</param>
        public void UpdateImage(VisualControlState state, bool isDark, Control? control)
        {
            if (svgImage is null)
                return;

            if (!HasImage(state, isDark))
            {
                SetImage(
                    state,
                    svgImage?.AsNormalImage(RealSvgHeight(control), isDark),
                    isDark);
            }
        }
    }
}
