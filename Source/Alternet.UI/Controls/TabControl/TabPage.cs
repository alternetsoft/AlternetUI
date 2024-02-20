using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a single tab page in a <see cref="TabControl"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="TabPage"/> controls represent the tabbed pages in a
    /// <see cref="TabControl"/> control.
    /// The tabs in a <see cref="TabControl"/> are part of the <see cref="TabControl"/> but
    /// not parts of the individual <see cref="TabPage"/> controls.
    /// </remarks>
    [ControlCategory("Hidden")]
    public partial class TabPage : Control
    {
        private string title;

        /// <summary>
        /// Initializes a new instance of <see cref="TabPage"/> class.
        /// </summary>
        public TabPage()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TabPage"/> class with the specified title.
        /// </summary>
        public TabPage(string? title)
        {
            this.title = title ?? string.Empty;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Title"/> property changes.
        /// </summary>
        public event EventHandler? TitleChanged;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.TabPage;

        /// <summary>
        /// Gets or sets the text to display on the tab.
        /// </summary>
        /// <value>The text to display on the tab.</value>
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (title == value)
                    return;

                title = value;
                RaiseTitleChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the zero-based index of the page within the <see cref="TabControl"/> control,
        /// or <see langword="null"/> if the item is not associated with
        /// a <see cref="TabControl"/> control.
        /// </summary>
        public int? Index { get; internal set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Title))
                return base.ToString() ?? nameof(TabPage);
            else
                return Title;
        }

        /// <summary>
        /// Called when the value of the <see cref="Title"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnTitleChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="TitleChanged"/> event and calls
        /// <see cref="OnTitleChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        private void RaiseTitleChanged(EventArgs e)
        {
            OnTitleChanged(e);
            TitleChanged?.Invoke(this, e);
        }
    }
}