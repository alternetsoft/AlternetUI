using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that displays a frame around a group of controls with an optional title.
    /// </summary>
    /// <remarks>
    /// The <see cref="GroupBox"/> displays a frame around a group of controls with or without a title.
    /// Use a <see cref="GroupBox"/> to logically group a collection of controls in a window.
    /// The group box is a container control that can be used to define groups of controls.
    /// The typical use for a group box is to contain a logical group of <see cref="RadioButton"/> controls.
    /// If you have two group boxes, each of which contain several option buttons (also known as radio buttons),
    /// each group of buttons is mutually exclusive, setting one option value per group.
    /// You can add controls to the <see cref="GroupBox"/> by using the <see cref="ICollection{T}.Add(T)"/>
    /// method of the <see cref="Control.Children"/> property.
    /// </remarks>
    public class GroupBox : Control
    {
        private string? title = null;

        /// <summary>
        /// Occurs when the value of the <see cref="Title"/> property changes.
        /// </summary>
        public event EventHandler? TitleChanged;

        /// <summary>
        /// Gets or sets the title text for this group box.
        /// </summary>
        /// <value>A title text string for this group box, or <c>null</c> if the group box has no title.</value>
        public string? Title
        {
            get
            {
                CheckDisposed();
                return title;
            }

            set
            {
                CheckDisposed();
                if (title == value)
                    return;

                title = value;
                RaiseTitleChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="Title"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnTitleChanged(EventArgs e)
        {
        }

        private void RaiseTitleChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTitleChanged(e);
            TitleChanged?.Invoke(this, e);
        }
    }
}