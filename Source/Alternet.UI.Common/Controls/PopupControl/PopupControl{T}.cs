using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a popup control that hosts a specific type of content control.
    /// </summary>
    /// <remarks>The <see cref="PopupControl{T}"/> class provides a generic
    /// implementation for creating and
    /// managing a popup control that hosts a specific type of content control.
    /// The content control is automatically
    /// instantiated and configured to inherit font settings and
    /// parent relationships from the popup.</remarks>
    /// <typeparam name="T">The type of the content control hosted by the popup.
    /// The type must derive from <see cref="AbstractControl"/> and
    /// have a parameterless constructor.</typeparam>
    public class PopupControl<T> : PopupControl
        where T : AbstractControl, new()
    {
        private readonly AbstractControl content;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupControl{T}"/> class.
        /// </summary>
        public PopupControl()
        {
            content = CreateContent();
            content.ParentFont = true;
            content.Parent = this;
        }

        /// <summary>
        /// Gets the content control of the popup, cast to the specified type.
        /// </summary>
        public T Content => (T)content;

        /// <summary>
        /// Creates and returns a new instance of the control represented
        /// by the generic type parameter <typeparamref name="T"/>.
        /// </summary>
        /// <returns>A new instance of <typeparamref name="T"/>.</returns>
        protected AbstractControl CreateContent() => new T();
    }
}
