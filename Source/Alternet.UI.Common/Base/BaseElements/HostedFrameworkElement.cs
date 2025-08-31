using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a framework element that keeps track of host objects and
    /// provides methods to manage them. This class is useful in scenarios where a framework element
    /// can be hosted within a user interface or needs
    /// to manage external resources or context-specific objects.
    /// </summary>
    /// <remarks>The <see cref="HostedFrameworkElement"/> class is designed
    /// to manage a collection of host objects, which can include references
    /// to native or managed controls. It provides methods to add, remove, retrieve,
    /// and dispose of these objects.
    /// <para> The collection of host objects is managed internally and can be accessed
    /// via the <see cref="HostObjects"/> property. Derived classes can override the provided
    /// methods to customize the behavior for managing host objects. </para></remarks>
    public partial class HostedFrameworkElement : FrameworkElement
    {
        private IList<object>? hostObjects;

        /// <summary>
        /// Gets list of host objects. For example, it can contain native object references or
        /// <see cref="IContextMenuHost"/> controls
        /// such as <see cref="PopupToolBar"/> or <see cref="InnerPopupToolBar"/>.
        /// </summary>
        [Browsable(false)]
        public IList<object>? HostObjects
        {
            get
            {
                return hostObjects;
            }
        }

        /// <summary>
        /// Retrieves the first host object of the specified type
        /// from the available host object.
        /// </summary>
        /// <remarks>This method iterates through the collection of host objects
        /// associated with this instance of <see cref="HostedFrameworkElement"/>
        /// and returns the first
        /// host that matches the specified type <typeparamref name="T"/>.
        /// If no matching host object is found, the
        /// method returns <see langword="null"/>.</remarks>
        /// <typeparam name="T">The type of the host object to retrieve.</typeparam>
        /// <returns>The first host object of type <typeparamref name="T"/> if found;
        /// otherwise, <see langword="null"/>.</returns>
        public virtual T? GetHostObject<T>()
            where T : class
        {
            if (HostObjects is not null)
            {
                foreach (var host in HostObjects)
                {
                    if (host is T typedHost)
                        return typedHost;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds a host object to the collection of context menu hosts if it is not already present.
        /// </summary>
        /// <remarks>This method ensures that the specified host object is added only once to the
        /// collection.</remarks>
        /// <param name="host">The host object to add. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="host"/>
        /// is <see langword="null"/>.</exception>
        public virtual void AddHostObject(object host)
        {
            if (host is null)
                throw new ArgumentNullException(nameof(host));
            if (hostObjects is null)
                hostObjects = new List<object>();
            if (!hostObjects.Contains(host))
                hostObjects.Add(host);
        }

        /// <summary>
        /// Removes the specified host object from the collection of host objects.
        /// </summary>
        /// <remarks>If the specified host object is not found in the collection, no action is
        /// taken.</remarks>
        /// <param name="host">The host object to remove. Must not be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the host object was found and removed;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool RemoveHostObject(object host)
        {
            return hostObjects?.Remove(host) ?? false;
        }

        /// <summary>
        /// Releases resources held by the host objects and clears the collection.
        /// </summary>
        /// <remarks>This method disposes of each object in the collection if it implements
        /// <see cref="IDisposable"/>, clears the collection, and sets the reference to null.
        /// Override this method in a derived class  to provide additional cleanup logic
        /// for host objects.</remarks>
        protected virtual void DisposeHostObjects()
        {
            if (hostObjects != null)
            {
                foreach (var host in hostObjects)
                {
                    if (host is IDisposable disposable)
                        disposable.Dispose();
                }

                hostObjects.Clear();
                hostObjects = null;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            DisposeHostObjects();
            base.DisposeManaged();
        }
    }
}
