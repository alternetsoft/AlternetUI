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

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a container for images. It can be used to store multiple images of various sizes and color depths.
    /// </summary>
    public partial class ImageContainer : ImmutableObject, IImageContainer
    {
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
        /// Gets whether this object is dummy and doesn't do anything.
        /// </summary>
        public virtual bool IsDummy => false;

        /// <summary>
        /// Gets whether object is readonly.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsReadOnly => Immutable;

        /// <summary>
        /// Gets the <see cref="Image"/> collection for this image list.
        /// </summary>
        /// <value>The collection of images.</value>
        public virtual BaseCollection<Image> Images { get; } = new(CollectionSecurityFlags.NoNullOrReplace);

        /// <summary>
        /// Retrieves an image from the container at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the image to retrieve.
        /// If the index is null, less than 0, or greater than or equal to the
        /// number of images, null is returned.</param>
        /// <returns>The <see cref="Image"/> at the specified index, or null if
        /// the index is invalid.</returns>
        public virtual Image? GetImage(int? index)
        {
            if (index < 0 || index >= Images.Count)
                return null;
            return Images[index];
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
        /// <returns></returns>
        /// <returns>True on success; False on failure.</returns>
        public virtual bool Remove(int imageIndex)
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

        /// <summary>
        /// Called when <see cref="Changed"/> event is raised.
        /// </summary>
        protected virtual void OnChanged()
        {
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
