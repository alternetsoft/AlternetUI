using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that displays a frame around a group of controls
    /// with an optional title.
    /// </summary>
    /// <remarks>
    /// The <see cref="GroupBox"/> displays a frame around a group of controls
    /// with or without a title.
    /// Use a <see cref="GroupBox"/> to logically group a collection of controls
    /// in a window.
    /// The group box is a container control that can be used to define groups
    /// of controls.
    /// The typical use for a group box is to contain a logical group of
    /// <see cref="RadioButton"/> controls.
    /// If you have two group boxes, each of which contain several option buttons
    /// (also known as radio buttons),
    /// each group of buttons is mutually exclusive, setting one option value per group.
    /// You can add controls to the <see cref="GroupBox"/> by using the Add
    /// method of the <see cref="Control.Children"/> property.
    /// </remarks>
    [DefaultEvent("Enter")]
    [DefaultProperty("Text")]
    [ControlCategory("Containers")]
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
        /// <value>A title text string for this group box, or <c>null</c> if
        /// the group box has no title.</value>
        public string? Title
        {
            get
            {
                return title;
            }

            set
            {
                if (title == value)
                    return;
                CheckDisposed();
                title = value;
                RaiseTitleChanged(EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.GroupBox;

        internal new Native.GroupBox NativeControl => (Native.GroupBox)base.NativeControl;

        /// <summary>
        /// Gets the top border ( it is the margin at the top where the title is).
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This is used to account for the
        /// need for extra space taken by the <see cref="GroupBox"/>.
        /// </remarks>
        public int GetTopBorderForSizer()
        {
            return NativeControl.GetTopBorderForSizer();
        }

        /// <summary>
        /// Gets the margin on all other sides except top side.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This is used to account for the
        /// need for extra space taken by the <see cref="GroupBox"/>.
        /// </remarks>
        public int GetOtherBorderForSizer()
        {
            return NativeControl.GetOtherBorderForSizer();
        }

        /// <summary>
        /// Called when the value of the <see cref="Title"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnTitleChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateGroupBoxHandler(this);
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