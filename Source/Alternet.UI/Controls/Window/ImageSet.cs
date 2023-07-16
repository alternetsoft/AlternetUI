using System;
using System.ComponentModel;
using System.IO;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// An image set to be used with UI elements (for example, <see cref="Window.Icon"/>).
    /// Can be used for specifying several size representations of the same picture.
    /// </summary>
    [TypeConverter(typeof(ImageSetConverter))]
    public class ImageSet : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> with default values.
        /// </summary>
        public ImageSet()
        {
            NativeImageSet = new UI.Native.ImageSet();

            Images.ItemInserted += Images_ItemInserted;
            Images.ItemRemoved += Images_ItemRemoved;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class from the specified data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the image.</param>
        public ImageSet(Stream stream)
            : this()
        {
            using var inputStream = new UI.Native.InputStream(stream);
            NativeImageSet.LoadFromStream(inputStream);
        }

        public static ImageSet? FromImageGrayScale(Image? image)
        {
            if (image == null)
                return null;

            ImageSet result = new();
            result.Images.Add(image!.ToGrayScale());
            return result;
        }

        public static ImageSet? FromImage(Image? image)
        {
            if (image == null)
                return null;

            ImageSet result = new();
            result.Images.Add(image);
            return result;
        }

        private void Images_ItemInserted(object? sender, CollectionChangeEventArgs<Image> e)
        {
            NativeImageSet.AddImage(e.Item.NativeImage);
            OnChanged();
        }

        private void Images_ItemRemoved(object? sender, CollectionChangeEventArgs<Image> e)
        {
            OnChanged();

            // todo
            throw new Exception();
        }

        private void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when the image set is changed, i.e. an image is added to it or removed from it.
        /// </summary>
        public event EventHandler? Changed;

        /// <summary>
        /// Gets the <see cref="Image"/> collection for this image list.
        /// </summary>
        /// <value>The collection of images.</value>
        public Collection<Image> Images { get; } = new Collection<Image>();

        internal UI.Native.ImageSet NativeImageSet { get; private set; }

        /// <summary>
        /// Releases all resources used by the <see cref="ImageList"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class
        /// from the specified url.
        /// </summary>
        /// <param name="url">The file or embedded resource url used to load the image.
        /// </param>
        /// <example>
        /// int ImageSize = 16;
        /// string ResPrefix =
        ///     $"embres:ControlsTest.resources.Png._{ImageSize}.";
        /// ImageSet imageSet = ImageSet.FromUrl($"{ResPrefix}arrow-left-{ImageSize}.png");
        /// </example>
        public static ImageSet FromUrl(string url)
        {
                var s = (string)url;
                var uri = s.StartsWith("/")
                    ? new Uri(s, UriKind.Relative)
                    : new Uri(s, UriKind.RelativeOrAbsolute);

                if (uri.IsAbsoluteUri && uri.IsFile)
                {
                    using var stream = File.OpenRead(uri.LocalPath);
                    return new ImageSet(stream);
                }

                var assets = new UI.ResourceLoader();
                return new ImageSet(assets.Open(uri));
        }

        public static ImageSet FromUrlOrNull(string url)
        {
            try
            {
                var result = ImageSet.FromUrl(url);
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ImageList"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativeImageSet.Dispose();
                    NativeImageSet = null!;
                }

                isDisposed = true;
            }
        }
    }
}