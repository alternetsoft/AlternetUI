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

            if (Handler != null && !Handler.Add(item))
                App.DebugLogError("Error adding image to container");
        }

        /// <inheritdoc/>
        protected override void OnImageRemoved(object? sender, int index, Image item)
        {
            base.OnImageRemoved(sender, index, item);

            if (Handler != null && !Handler.Remove(index))
                App.DebugLogError("Error removing image from container");
        }
    }
}
