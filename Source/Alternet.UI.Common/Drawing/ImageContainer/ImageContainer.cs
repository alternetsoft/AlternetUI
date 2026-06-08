using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Alternet.Base.Collections;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a container for images. It can be used to store multiple images of various sizes and color depths.
    /// </summary>
    public partial class ImageContainer : ImmutableObject, IImageContainer, IComparer<SizeI>
    {
        /// <summary>
        /// Gets or sets the default size of the images in the image container, in pixels.
        /// </summary>
        public static int DefaultImageSize = 16;

        private int suspendImagesEvents;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageContainer"/> class with the specified immutability.
        /// </summary>
        /// <param name="immutable">Whether this object is immutable (properties are readonly).</param>
        public ImageContainer(bool immutable)
            : this()
        {
            SetImmutable(immutable);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageContainer"/> class.
        /// </summary>
        public ImageContainer()
        {
            Images.ItemInserted += OnImageInserted;
            Images.ItemRemoved += OnImageRemoved;
        }

        /// <summary>
        /// Occurs when object is changed (image is added or removed).
        /// </summary>
        public event EventHandler? Changed;

        /// <summary>
        /// Gets whether object is ok.
        /// </summary>
        /// <summary>
        /// Gets whether container instance is valid and contains image(s).
        /// </summary>
        [Browsable(false)]
        public virtual bool IsOk => Images.Count > 0;

        /// <summary>
        /// Gets whether object is readonly.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsReadOnly => Immutable;

        /// <summary>
        /// Gets image with size equal to system icon size.
        /// </summary>
        [Browsable(false)]
        public virtual Image? ImageWithSystemIconSize => GetExactImage(IconSet.EffectiveSystemIconSize);

        /// <summary>
        /// Gets whether container contains an image with the system icon size.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasImageWithSystemIconSize => ImageWithSystemIconSize != null;

        /// <summary>
        /// Gets image with size equal to small system icon size.
        /// </summary>
        [Browsable(false)]
        public virtual Image? ImageWithSmallSystemIconSize => GetExactImage(IconSet.EffectiveSmallSystemIconSize);

        /// <summary>
        /// Gets whether container contains an image with the small system icon size.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasImageWithSmallSystemIconSize => ImageWithSmallSystemIconSize != null;

        /// <summary>
        /// Gets the first image in the container or null if the container is empty.
        /// Result is returned as immutable image if the container is immutable.
        /// </summary>
        [Browsable(false)]
        public Image? FirstImage => MakeImmutableIfNeeded(Images.Count > 0 ? Images[0] : null);

        /// <summary>
        /// Gets the first image in the container which has the system icon size.
        /// Result is returned as immutable image if the container is immutable.
        /// </summary>
        [Browsable(false)]
        public Image? ImageOfSystemIconSize
        {
            get
            {
                return GetExactImage(IconSet.EffectiveSystemIconSize);
            }
        }

        /// <summary>
        /// Gets the smallest image in the container.
        /// Result is returned as immutable image if the container is immutable.
        /// </summary>
        [Browsable(false)]
        public Image? SmallestImage
        {
            get
            {
                if (Images.Count == 0)
                    return null;

                var image = Images[0];

                for (int i = 1; i < Images.Count; i++)
                {
                    if (IsSmallerThan(Images[i], image))
                        image = Images[i];
                }

                return MakeImmutableIfNeeded(image);
            }
        }

        /// <summary>
        /// Gets the first image in the container which has the small system icon size.
        /// Result is returned as immutable image if the container is immutable.
        /// </summary>
        [Browsable(false)]
        public Image? ImageOfSmallSystemIconSize
        {
            get
            {
                return GetExactImage(IconSet.EffectiveSmallSystemIconSize);
            }
        }

        /// <summary>
        /// Gets the <see cref="Image"/> collection for this image list.
        /// </summary>
        /// <value>The collection of images.</value>
        public virtual BaseCollection<Image> Images { get; } = new(CollectionSecurityFlags.NoNullOrReplace);

        /// <summary>
        /// Gets suggested size of the image for the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">Scale factor for which to get suggested size of the image.</param>
        /// <param name="baseSize">Base size of the image. Default is 16.</param>
        /// <returns>Suggested size of the image for the specified scale factor.</returns>
        public static int GetSuggestedSize(Coord scaleFactor, int baseSize = 16)
        {
            int size = baseSize;

            if (scaleFactor > 1)
            {
                size = (int)(size * scaleFactor);
                if (size < 32)
                    size = 32;
            }

            return size;
        }

        /// <summary>
        /// Retrieves an image from the container at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the image to retrieve.
        /// If the index is null, less than 0, or greater than or equal to the
        /// number of images, null is returned.</param>
        /// <returns>The <see cref="Image"/> at the specified index, or null if
        /// the index is invalid. Result is returned as immutable image if the container is immutable.</returns>
        public virtual Image? GetImageAt(int? index)
        {
            if (index < 0 || index >= Images.Count)
                return null;
            return MakeImmutableIfNeeded(Images[index]);
        }

        /// <summary>
        /// Raises <see cref="Changed"/> event and <see cref="OnChanged"/> method.
        /// </summary>
        public void RaiseChanged()
        {
            OnChanged();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Removes image from the container.
        /// </summary>
        /// <param name="imageIndex">Index of image to remove.</param>
        /// <returns>True on success; False on failure.</returns>
        public virtual bool RemoveAt(int imageIndex)
        {
            if (imageIndex < 0 || imageIndex >= Images.Count)
                return false;
            Images.RemoveAt(imageIndex);
            return true;
        }

        /// <summary>
        /// Adds image from the specified assembly and relative path to the resource.
        /// </summary>
        /// <param name="asm">Assembly to load image from.</param>
        /// <param name="name">Image name or relative path.
        /// Slash characters will be changed to '.'.
        /// Example: "ToolBarPng/Large\Calendar32.png" -> "ToolBarPng.Large.Calendar32.png".
        /// </param>
        /// <returns></returns>
        public virtual bool AddFromAssemblyUrl(Assembly asm, string? name = null)
        {
            string url = AssemblyUtils.GetImageUrlInAssembly(asm, name);
            return AddFromUrl(url);
        }

        /// <summary>
        /// Compares two images and returns a value indicating whether the first image is smaller than the second image.
        /// This method is used for sorting images by size.
        /// </summary>
        /// <param name="image1">The first image to compare.</param>
        /// <param name="image2">The second image to compare.</param>
        /// <returns><c>true</c> if the first image is smaller than the second image; otherwise, <c>false</c>.</returns>
        public virtual bool IsSmallerThan(Image image1, Image image2)
        {
            return IsSmallerThan(image1.Size, image2.Size);
        }

        /// <summary>
        /// Compares two sizes and returns a value indicating whether the first size is smaller than the second size.
        /// This method is used for sorting images by size.
        /// </summary>
        /// <param name="size1">The first size to compare.</param>
        /// <param name="size2">The second size to compare.</param>
        /// <returns><c>true</c> if the first size is smaller than the second size; otherwise, <c>false</c>.</returns>
        public virtual bool IsSmallerThan(SizeI size1, SizeI size2)
        {
            int h1 = size1.Height;
            int h2 = size2.Height;
            return h1 < h2 || (h1 == h2 && size1.Width < size2.Width);
        }

        /// <summary>
        /// Gets the image from the container that is the closest in size to the specified size.
        /// If there are multiple images of the same size, the first one is returned.
        /// If there are no images in the container, an empty <see cref="Bitmap"/> is returned.
        /// </summary>
        /// <param name="size">The target size to find the closest image for.</param>
        /// <returns>The image that is closest in size to the specified size.
        /// Result is returned as immutable image if the container is immutable.</returns>
        public virtual Image AsImage(SizeI size)
        {
            return GetClosestImage(size) ?? Bitmap.Empty;
        }

        /// <summary>
        /// Gets the image from the container that has the specified size, or null if no such image exists.
        /// </summary>
        /// <param name="size">The target size to find the exact image for.</param>
        /// <returns>The image that has the specified size, or null if no such image exists.
        /// Result is returned as immutable image if the container is immutable.</returns>
        public virtual Image? GetExactImage(SizeI size)
        {
            var result = Images.FirstOrDefault(i => i.Size == size);
            return MakeImmutableIfNeeded(result);
        }

        /// <summary>
        /// Gets the image from the container that has the specified size, or null if no such image exists.
        /// </summary>
        /// <param name="size">The target size to find the exact image for.</param>
        /// <param name="fallbacks">An array of fallback options to use if the exact image is not found.</param>
        /// <returns>The image that has the specified size, or null if no such image exists.
        /// Result is returned as immutable image if the container is immutable.</returns>
        public virtual Image? GetExactOrClosestImage(SizeI size, params ImageSizeFallback[] fallbacks)
        {
            var result = GetExactImage(size);

            if (result != null)
                return result;

            foreach (var fallback in fallbacks)
            {
                switch (fallback)
                {
                    case ImageSizeFallback.SystemIcon:
                        result = ImageOfSystemIconSize;
                        break;
                    case ImageSizeFallback.SmallSystemIcon:
                        result = ImageOfSmallSystemIconSize;
                        break;
                    case ImageSizeFallback.NearestLarger:
                        result = GetNearestLargerImage(size);
                        break;
                    case ImageSizeFallback.Closest:
                        result = GetClosestImage(size);
                        break;
                    case ImageSizeFallback.Smallest:
                        result = SmallestImage;
                        break;
                    case ImageSizeFallback.First:
                        result = FirstImage;
                        break;
                }

                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// Get the image from the container that has the specified size,
        /// or the scaled closest image if no exact match is found.
        /// </summary>
        /// <param name="size">The target size to find the image for.</param>
        /// <param name="addScaled">Indicates whether to add the scaled image
        /// to the container if no exact match is found.</param>
        /// <param name="downscaleFirst">Indicates whether to prioritize downscaling
        /// over upscaling when no exact match is found.</param>
        /// <returns>The image that has the specified size, or the scaled closest
        /// image if no exact match is found.</returns>
        /// <remarks>
        /// <para>
        /// <b>Downscaling</b><br/>
        ///   Pros: Preserves sharpness, smaller file size, faster load times<br/>
        ///   Cons: Some fine detail lost, compression artifacts possible<br/>
        ///   Best Use Cases: Web optimization, thumbnails, email attachments
        /// </para>
        /// <para>
        /// <b>Upscaling</b><br/>
        ///   Pros: Larger display size, needed for print or high‑res screens<br/>
        ///   Cons: Pixelation, blur, no new detail created, quality drops beyond ~150–200%<br/>
        ///   Best Use Cases: Posters, large displays, when only low‑res source exists
        /// </para>
        /// </remarks>
        public virtual Image? GetExactOrScaledImage(SizeI size, bool downscaleFirst, bool addScaled)
        {
            var result = GetExactImage(size);
            if (result != null)
                return result;
            if (downscaleFirst)
            {
                result = GetNearestLargerImage(size) ?? GetNearestSmallerImage(size);
            }
            else
            {
                result = GetNearestSmallerImage(size) ?? GetNearestLargerImage(size);
            }

            return result;
        }

        /// <summary>
        /// Get the image from the container that has the specified size,
        /// or the upscaled closest smaller image if no exact match is found.
        /// </summary>
        /// <param name="size">The target size to find the image for.</param>
        /// <param name="addScaled">Indicates whether to add the scaled image
        /// to the container if no exact match is found.</param>
        /// <returns>The image that has the specified size, or the upscaled closest
        /// smaller image if no exact match is found.</returns>
        public virtual Image? GetExactOrUpscaleSmallerImage(SizeI size, bool addScaled)
        {
            var result = GetExactImage(size);
            if (result != null)
                return result;
            result = GetNearestSmallerImage(size);
            if (result != null)
            {
                result = new Bitmap(result, size);
                if (addScaled && !IsReadOnly)
                    Add(result);
                return result;
            }
            return null;
        }

        /// <summary>
        /// Get the image from the container that has the specified size,
        /// or the downscaled closest larger image if no exact match is found.
        /// </summary>
        /// <param name="size">The target size to find the image for.</param>
        /// <param name="addScaled">Indicates whether to add the scaled image
        /// to the container if no exact match is found.</param>
        /// <returns>The image that has the specified size, or the downscaled closest
        /// larger image if no exact match is found.</returns>
        public virtual Image? GetExactOrDownscaleLargerImage(SizeI size, bool addScaled)
        {
            var result = GetExactImage(size);
            if (result != null)
                return result;

            var larger = GetNearestLargerImage(size);
            if (larger != null)
            {
                result = new Bitmap(larger, size);
                if (addScaled && !IsReadOnly)
                    Add(result);
                return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the image from the container that is smaller than the specified size,
        /// or null if no such image exists.
        /// </summary>
        /// <param name="size">The target size to find the nearest smaller image for.</param>
        /// <returns>The image that is smaller than the specified size, or null if no such image exists.
        /// Result is returned as immutable image if the container is immutable.</returns>
        public virtual Image? GetNearestSmallerImage(SizeI size)
        {
            Image? result = null;

            foreach (var bitmap in Images)
            {
                if (IsSmallerThan(bitmap.Size, size))
                {
                    if (result is null)
                        result = bitmap;
                    else
                    {
                        var newDistance = SizeI.Subtract(bitmap.Size, size).Abs;
                        var oldDistance = SizeI.Subtract(result.Size, size).Abs;

                        if (newDistance.Width < oldDistance.Width
                            && newDistance.Height < oldDistance.Height)
                            result = bitmap;
                    }
                }
            }

            return MakeImmutableIfNeeded(result);
        }

        /// <summary>
        /// Gets the image from the container that is larger than the specified size,
        /// or null if no such image exists.
        /// </summary>
        /// <param name="size">The target size to find the nearest larger image for.</param>
        /// <returns>The image that is larger than the specified size, or null if no such image exists.
        /// Result is returned as immutable image if the container is immutable.</returns>
        public virtual Image? GetNearestLargerImage(SizeI size)
        {
            Image? result = null;

            foreach (var bitmap in Images)
            {
                if (IsSmallerThan(bitmap.Size, size))
                    continue;

                if (result is null)
                    result = bitmap;
                else
                {
                    var newDistance = SizeI.Subtract(bitmap.Size, size).Abs;
                    var oldDistance = SizeI.Subtract(result.Size, size).Abs;

                    if (newDistance.Width < oldDistance.Width
                        && newDistance.Height < oldDistance.Height)
                        result = bitmap;
                }
            }

            return MakeImmutableIfNeeded(result);
        }

        /// <summary>
        /// Gets the image from the container that is the closest to the specified size.
        /// If there are multiple images of the same size, the first one is returned.
        /// If there are no images in the container, null is returned.
        /// </summary>
        /// <param name="size">The target size to find the closest image for.</param>
        /// <returns>The image that is closest in size to the specified size.
        /// Result is returned as immutable image if the container is immutable.</returns>
        public virtual Image? GetClosestImage(SizeI size)
        {
            Image? result = null;

            foreach (var bitmap in Images)
            {
                if (result is null)
                    result = bitmap;
                else
                {
                    var newDistance = SizeI.Subtract(bitmap.Size, size).Abs;
                    var oldDistance = SizeI.Subtract(result.Size, size).Abs;

                    if (newDistance.Width < oldDistance.Width
                        && newDistance.Height < oldDistance.Height)
                        result = bitmap;
                }
            }

            result ??= Images.First();

            return MakeImmutableIfNeeded(result);
        }

        /// <summary>
        /// Adds image from the specified resource url.
        /// See <see cref="Image.FromUrl"/> for the url format example.
        /// </summary>
        /// <param name="url">The file or embedded resource url used to load the image.</param>
        /// <returns>True on success; False on failure.</returns>
        public virtual bool AddFromUrl(string? url)
        {
            if (IsReadOnly || url is null)
                return false;

            var image = Image.FromUrlOrNull(url);
            if (image is null || !image.IsOk)
                return false;
            Images.Add(image);
            return true;
        }

        /// <summary>
        /// Adds image.
        /// </summary>
        /// <param name="image">Image to add.</param>
        /// <returns>True on success; False on failure.</returns>
        public virtual bool Add(Image? image)
        {
            if (IsReadOnly || image is null)
                return false;
            Images.Add(image);
            return true;
        }

        /// <summary>
        /// Adds image from the stream.
        /// </summary>
        /// <param name="stream">Stream with image.</param>
        public virtual bool Add(Stream? stream)
        {
            if (IsReadOnly || stream is null)
                return false;
            var image = new Bitmap(stream);
            return Add(image);
        }

        /// <summary>
        /// Exports all images in the collection to files at the specified base path, using the provided file names or
        /// default names if none are specified.
        /// </summary>
        /// <remarks>If a file name does not have a supported image extension (.png, .jpg, .jpeg, .bmp),
        /// the method appends '.png' by default. Existing files with the same names may be overwritten.</remarks>
        /// <param name="basePath">The directory path where the image files will be saved. Must be a valid,
        /// writable file system path.</param>
        /// <param name="fileNames">An array of file names to use for the exported images. If the array contains fewer names than images,
        /// default names in the format 'ImageN.png' will be used for remaining images.</param>
        public virtual void ExportImagesToFiles(string basePath, IReadOnlyList<string> fileNames)
        {
            for (int i = 0; i < Images.Count; i++)
            {
                var image = Images[i];
                var fileName = fileNames.Count > i ? fileNames[i] : $"Image{i}.png";

                var extension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
                if (extension != ".png" && extension != ".jpg" && extension != ".jpeg" && extension != ".bmp")
                    fileName += ".png";

                var filePath = System.IO.Path.Combine(basePath, fileName);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                image.Save(filePath);
            }
        }

        /// <summary>
        /// Adds svg to the image container with the default normal color for the specified color theme.
        /// </summary>
        /// <param name="svg">The svg image to add.</param>
        /// <param name="imageSize">The size of the image to be added.</param>
        /// <param name="isDarkTheme">Whether theme is dark.</param>
        /// <returns>The added <see cref="Image"/> if successful; otherwise, <see langword="null"/>.</returns>
        public virtual Image? AddSvg(SvgImage svg, SizeI imageSize, bool isDarkTheme)
        {
            if (IsReadOnly)
                return null;

            var color = svg.GetSvgColor(KnownSvgColor.Normal, isDarkTheme);
            var result = AddSvg(svg, imageSize, color);
            return result;
        }

        /// <summary>
        /// Adds svg to the image container with the specified color or default color for the specified theme.
        /// If <paramref name="isDarkTheme"/> is not null, it is used to determine the svg color.
        /// Otherwise, if <paramref name="color"/> is not null, it is used as the svg color.
        /// If both parameters are null, the default color of the svg is used.
        /// </summary>
        /// <param name="svg">The svg image to add.</param>
        /// <param name="imageSize">The size of the image to be added.</param>
        /// <param name="isDarkTheme">Whether theme is dark. Optional.</param>
        /// <param name="color">The color to apply to the SVG image. Optional.</param>
        /// <returns>The added <see cref="Image"/> if successful; otherwise, <see langword="null"/>.</returns>
        public virtual Image? AddSvg(SvgImage svg, SizeI imageSize, bool? isDarkTheme = null, Color? color = null)
        {
            if (IsReadOnly)
                return null;

            if (isDarkTheme.HasValue)
                return AddSvg(svg, imageSize, isDarkTheme.Value);

            return AddSvg(svg, imageSize, color);
        }

        /// <summary>
        /// Adds svg to the image container with the specified color.
        /// </summary>
        /// <param name="svg">The svg image to add.</param>
        /// <param name="imageSize">The size of the image to be added.</param>
        /// <param name="color">Svg color. Optional. If not specified, svg colors
        /// are not changed.</param>
        /// <returns>The added <see cref="Image"/> if successful; otherwise, <see langword="null"/>.</returns>
        public virtual Image? AddSvg(SvgImage svg, SizeI imageSize, Color? color = null)
        {
            if (IsReadOnly)
                return null;

            if (imageSize.SameWidthHeight)
            {
                var image = svg.ImageWithColor(imageSize.Width, color);
                Add(image);
                return image;
            }
            else
            {
                var imageSet = svg.CreateImageSet(imageSize, color);
                if (imageSet is null)
                    return null;
                var image = imageSet.AsImage(imageSize);
                Add(image);
                return image;
            }
        }

        /// <summary>
        /// Adds the image to the image container, resizing it if necessary.
        /// If image size equal to the specified size, it is added without resizing.
        /// </summary>
        /// <param name="image">The image to be added.</param>
        /// <param name="imageSize">The size to which the image should be resized if necessary.</param>
        /// <returns>Returns true if the image was added successfully.</returns>
        public virtual bool AddResized(Image image, SizeI imageSize)
        {
            if (IsReadOnly)
                return false;

            if (image.Size != imageSize)
            {
                var resizedImage = new Bitmap(image, imageSize);
                return Add(resizedImage);
            }

            return Add(image);
        }

        /// <summary>
        /// Creates a single image strip by combining all images in the collection horizontally.
        /// </summary>
        /// <remarks>Each image in the collection is placed side by side in the resulting strip,
        /// maintaining its original size and order. The width of the strip is the sum of the widths of all images, and
        /// the height matches the individual image height. This method is useful for generating sprite sheets or
        /// preview strips from a sequence of images.</remarks>
        /// <returns>An <see cref="Image"/> containing the combined image strip, or <see langword="null"/> if the collection
        /// contains no images.</returns>
        /// <param name="imageSize">The size of each individual image in the strip.</param>
        public virtual Image? AsImageStrip(SizeI imageSize)
        {
            var result = AsSkiaStrip(imageSize);
            if (result == null)
                return null;

            return (Image)result;
        }

        /// <summary>
        /// Creates a single horizontal bitmap strip by concatenating all images in the collection.
        /// </summary>
        /// <remarks>The resulting bitmap arranges each image side by side in the order they appear in the
        /// collection. The width of the strip is the sum of the widths of all images, and the height matches the
        /// individual image height. The caller is responsible for disposing the returned <see cref="SKBitmap"/> when it
        /// is no longer needed.</remarks>
        /// <returns>An <see cref="SKBitmap"/> representing the concatenated image strip, or <see langword="null"/> if the
        /// collection contains no images.</returns>
        /// <param name="imageSize">The size of each individual image in the strip.</param>
        public virtual SKBitmap? AsSkiaStrip(SizeI imageSize)
        {
            if (Images.Count == 0)
                return null;
            var width = imageSize.Width;
            var height = imageSize.Height;
            var stripWidth = width * Images.Count;

            var stripImage = new SKBitmap(stripWidth, height, false);
            var canvas = new SKCanvas(stripImage);

            for (int i = 0; i < Images.Count; i++)
            {
                var image = Images[i];
                var x = i * width;
                canvas.DrawBitmap((SKBitmap)image, x, 0);
            }

            return stripImage;
        }

        /// <summary>
        /// Adds image strip to the image container. The image strip is a single image
        /// that contains multiple images concatenated horizontally.
        /// This method divides the image strip into individual images based on the <paramref name="imageSize"/>
        /// parameter and adds them to the container.
        /// </summary>
        /// <param name="strip">The image strip containing multiple images.</param>
        /// <param name="imageSize">The size of each individual image in the strip.</param>
        public virtual bool AddImageStrip(Image strip, SizeI imageSize)
        {
            if (IsReadOnly)
                return false;

            var width = imageSize.Width;
            int imageCount = strip.Width / width;
            for (int i = 0; i < imageCount; i++)
            {
                var sourceRectangle = new RectI(i * width, 0, width, imageSize.Height);
                var image = strip.GetSubBitmap(sourceRectangle);
                Add(image);
            }
            
            return true;
        }

        /// <summary>
        /// Enumerates the images in the container, yielding size of each image.
        /// Result is optionally ordered by size.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="SizeI"/> representing the size of each image.</returns>
        public virtual IEnumerable<SizeI> GetImageSizes(bool? sortDescending = null)
        {
            var sizes = Images.Select(image => image.Size);
            
            if (sortDescending.HasValue)
            {
                sizes = sortDescending.Value
                    ? sizes.OrderByDescending(s => s, this) : sizes.OrderBy(s => s, this);
            }

            return sizes;
        }

        /// <summary>
        /// Gets the minimal size of the bitmaps in this image container.
        /// If there are no bitmaps, returns <see cref="ImageContainer.DefaultImageSize"/>.
        /// </summary>
        /// <returns>The size of the smallest bitmap in the container.</returns>
        public virtual SizeI GetMinSize()
        {
            if (Images.Count == 0)
                return DefaultImageSize;

            var image = Images[0];

            for (int i = 1; i < Images.Count; i++)
            {
                if (IsSmallerThan(Images[i], image))
                    image = Images[i];
            }

            return image.Size;
        }

        /// <summary>
        /// Removes all images from the container.
        /// </summary>
        public virtual bool Clear()
        {
            if (IsReadOnly)
                return false;
            suspendImagesEvents++;
            try
            {
                Images.Clear();
                RaiseChanged();
                return true;
            }
            finally
            {
                suspendImagesEvents--;
            }
        }

        int IComparer<SizeI>.Compare(SizeI x, SizeI y)
        {
            if (x == y)
                return 0;

            if (IsSmallerThan(x, y))
                return -1;

            return 1;
        }

        /// <summary>
        /// Draws an image on the specified graphic surface at the location specified
        /// by a coordinate pair.
        /// </summary>
        /// <param name="g">The Graphics object to draw on.</param>
        /// <param name="x">X-coordinate of the upper-left corner of the image to be drawn.</param>
        /// <param name="y">Y-coordinate of the upper-left corner of the image to be drawn.</param>
        /// <param name="index">Index of image to draw within image list.</param>
        public virtual bool Draw(Graphics g, Coord x, Coord y, int index)
        {
            if (index < 0 || index >= Images.Count)
                return false;

            var image = Images[index];

            if (image is null)
                return false;

            g.DrawImageUnscaled(image, new(x, y));
            return true;
        }

        /// <summary>
        /// Called when <see cref="Changed"/> event is raised.
        /// </summary>
        protected virtual void OnChanged()
        {
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            suspendImagesEvents++;
            Images.Clear();
            base.DisposeManaged();
        }

        /// <summary>
        /// Makes image immutable if the container is immutable.
        /// </summary>
        /// <param name="image">The image to potentially make immutable.</param>
        /// <returns>The original image, potentially made immutable.</returns>
        protected virtual Image? MakeImmutableIfNeeded(Image? image)
        {
            if (Immutable)
                image?.SetImmutable();
            return image;
        }

        /// <summary>
        /// Called when image is inserted in the container.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="index">The index at which the image was inserted.</param>
        /// <param name="item">The image that was inserted.</param>
        protected virtual void OnImageInserted(object? sender, int index, Image item)
        {
            if (suspendImagesEvents > 0)
                return;
            RaiseChanged();
        }

        /// <summary>
        /// Called when image is removed from the container.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="index">The index at which the image was removed.</param>
        /// <param name="item">The image that was removed.</param>
        protected virtual void OnImageRemoved(object? sender, int index, Image item)
        {
            if (suspendImagesEvents > 0)
                return;
            RaiseChanged();
        }
    }
}
