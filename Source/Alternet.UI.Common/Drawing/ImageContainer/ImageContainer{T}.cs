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
    /// Base class for the image containers.
    /// </summary>
    /// <typeparam name="T">Type of the handler.</typeparam>
    public abstract class ImageContainer<T> : HandledObject<T>, IImageContainer
        where T : class, IImageContainer
    {
        private int suspendImagesEvents;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageContainer{T}"/> class.
        /// </summary>
        /// <param name="immutable">Whether this object is immutable (properties are readonly).</param>
        protected ImageContainer(bool immutable)
            : base(immutable)
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
        public virtual bool IsOk => Handler.IsOk && Images.Count > 0;

        /// <summary>
        /// Gets whether this object is dummy and doesn't do anything.
        /// </summary>
        public virtual bool IsDummy => false;

        /// <summary>
        /// Gets the <see cref="Image"/> collection for this image list.
        /// </summary>
        /// <value>The collection of images.</value>
        public virtual BaseCollection<Image> Images { get; } = new(CollectionSecurityFlags.NoNull);

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
        /// Removes all images from the container.
        /// </summary>
        public virtual bool Clear()
        {
            if (IsReadOnly)
                return false;
            suspendImagesEvents++;
            try
            {
                var result = Handler.Clear();
                OnChanged();
                return result;
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
        /// <param name="sender"></param>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected virtual void OnImageInserted(object? sender, int index, Image item)
        {
            if (suspendImagesEvents > 0)
                return;
            if (!Handler.Add(item))
                App.DebugLogError("Error adding image to container");
            RaiseChanged();
        }

        /// <summary>
        /// Called when image is removed from the container.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected virtual void OnImageRemoved(object? sender, int index, Image item)
        {
            if (suspendImagesEvents > 0)
                return;
            if (!Handler.Remove(index))
                App.DebugLogError("Error removing image from container");
            RaiseChanged();
        }
    }
}
