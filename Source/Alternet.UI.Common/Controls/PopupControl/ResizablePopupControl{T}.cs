using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a resizable popup control with a window like border and title bar.
    /// This popup control is supposed to be shown inside the client area of another control.
    /// It is a specialized version of <see cref="ResizablePopupControl"/> that provides additional
    /// functionality for hosting a specific type of content control.
    /// </summary>
    /// <remarks>The <see cref="ResizablePopupControl{T}"/> class provides a generic
    /// implementation for creating and
    /// managing a popup control that hosts a specific type of content control.
    /// The content control is automatically
    /// instantiated and configured to inherit font settings and
    /// parent relationships from the popup.</remarks>
    /// <typeparam name="T">The type of the content control hosted by the popup.
    /// The type must derive from <see cref="AbstractControl"/> and
    /// have a parameterless constructor.</typeparam>
    public class ResizablePopupControl<T> : ResizablePopupControl
        where T : AbstractControl, new()
    {
        private readonly AbstractControl content;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizablePopupControl{T}"/> class.
        /// </summary>
        public ResizablePopupControl(bool useScrollViewer)
        {
            content = CreateContent();
            content.ParentFont = true;

            if (useScrollViewer)
            {
                content.Parent = ScrollViewer.Content;
            }
            else
            {
                content.VerticalAlignment = VerticalAlignment.Fill;
                content.Parent = BorderControl.FillPanel;
            }
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
