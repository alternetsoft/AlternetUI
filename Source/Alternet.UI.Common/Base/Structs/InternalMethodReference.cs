using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a reference to an internal method, encapsulating the ability
    /// to dynamically resolve and retrieve a delegate of the specified type.
    /// </summary>
    /// <remarks>This structure is designed to facilitate the dynamic resolution
    /// of internal methods, such as
    /// those exposed by external libraries, and provides caching to optimize
    /// subsequent delegate retrievals. It is
    /// particularly useful in scenarios where internal methods are resolved at runtime and invoked through
    /// delegates.</remarks>
    /// <typeparam name="TDelegate">The type of the delegate
    /// representing the internal method. Must derive from <see cref="Delegate"/>.</typeparam>
    public struct InternalMethodReference<TDelegate>
        where TDelegate : Delegate
    {
        private readonly string name;
        private readonly Type? container;
        private TDelegate? internalMethod;
        private bool loaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalMethodReference{TDelegate}"/>
        /// structure with the specified container type and method name.
        /// </summary>
        /// <param name="container">The type that contains the method.
        /// Can be <see langword="null"/> if the method is not associated with a
        /// specific type.</param>
        /// <param name="name">The name of the method being referenced.
        /// Cannot be <see langword="null"/> or empty.</param>
        public InternalMethodReference(Type? container, string name)
        {
            this.name = name;
            this.container = container;
        }

        /// <summary>
        /// Retrieves a delegate of the specified type, representing a native method, if available.
        /// </summary>
        /// <remarks>This method attempts to load a delegate for a native method by
        /// dynamically resolving
        /// the method from the SkiaSharp API. If the delegate has already been loaded,
        /// it is returned immediately.
        /// Otherwise, the method resolves the native method, creates the delegate,
        /// and caches it for future
        /// use.</remarks>
        /// <returns>The delegate of type <typeparamref name="TDelegate"/> representing
        /// the native method, or <see
        /// langword="null"/> if the method cannot be resolved.</returns>
        public TDelegate? GetDelegate()
        {
            if (loaded)
                return internalMethod;
            loaded = true;

            if (container is null)
                return null;

            var method = container.GetMethod(
                name,
                BindingFlags.NonPublic | BindingFlags.Static);

            var result = (TDelegate?)Delegate.CreateDelegate(typeof(TDelegate), method);
            internalMethod = result;
            return result;
        }
    }
}
