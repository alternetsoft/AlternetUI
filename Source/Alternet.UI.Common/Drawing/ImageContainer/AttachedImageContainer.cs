using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents an image container which is attached to another image container and forwards all calls to it.
    /// </summary>
    public class AttachedImageContainer<T> : ImageContainer
        where T : IImageContainer
    {
        private T? handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachedImageContainer{T}"/> class with the specified handler.
        /// </summary>
        /// <param name="handler">The handler to be attached.</param>
        public AttachedImageContainer(T? handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public override bool IsReadOnly => base.IsReadOnly || Handler?.IsReadOnly == true;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachedImageContainer{T}"/> class.
        /// </summary>
        public AttachedImageContainer()
        {
            handler = CreateHandler();
        }

        /// <summary>
        /// Gets handler of this image container.
        /// </summary>
        public T? Handler => handler;

        /// <summary>
        /// Creates the handler for this image container.
        /// </summary>
        /// <returns></returns>
        protected virtual T? CreateHandler() => default;

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDispose(ref handler);
            base.DisposeManaged();
        }

        /// <inheritdoc/>
        protected override void OnImageInserted(object? sender, int index, Image item)
        {
            base.OnImageInserted(sender, index, item);

            if (IsReadOnly)
            {
                App.DebugLogError("Failed to add image in read-only container");
                return;
            }

            if (Handler != null)
            {
                if (!Handler.Add(item))
                {
                    App.DebugLogError("Cannot add image to attached handler ");
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnImageRemoved(object? sender, int index, Image item)
        {
            base.OnImageRemoved(sender, index, item);

            if (IsReadOnly)
            {
                App.DebugLogError("Failed to remove image in read-only container");
                return;
            }

            if (Handler != null)
            {
                if (!Handler.RemoveAt(index))
                {
                    if (Handler.Clear())
                    {
                        foreach (var image in Images)
                            Handler.Add(image);
                    }
                    else
                    {
                        App.DebugLogError("Cannot remove image from attached handler ");
                    }
                }
            }
        }
    }
}
