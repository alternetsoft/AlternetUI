using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a url label control.
    /// </summary>
    [DefaultProperty("Text")]
    [DefaultBindingProperty("Text")]
    [ControlCategory("Other")]
    public partial class LinkLabel : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkLabel"/> class.
        /// </summary>
        public LinkLabel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkLabel"/> class with the specified text.
        /// </summary>
        public LinkLabel(string? text)
        {
            Text = text ?? string.Empty;
        }

        /// <summary>
        /// Occurs when a link is clicked within the control.
        /// </summary>
        public event CancelEventHandler? LinkClicked;

        /// <summary>
        /// Gets or sets whether to use generic or native control for <see cref="LinkLabel"/>.
        /// </summary>
        public static bool UseGenericControl { get; set; }

        /// <summary>
        /// Gets or sets whether to use our <see cref="AppUtils.ShellExecute"/>
        /// for opening the <see cref="Url"/> or allow native control to open it.
        /// </summary>
        /// <remarks>
        /// On Linux (Ubuntu 23) native control has errors with opening urls, so
        /// this property is <c>true</c> by default and we do not use it.
        /// </remarks>
        public static bool UseShellExecute { get; set; } = true;

        /// <inheritdoc/>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                value ??= " ";
                if (Text == value)
                    return;
                base.Text = value;
                Handler.Text = value;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.LinkLabel;

        /// <summary>
        /// Gets or sets the URL associated with the hyperlink.
        /// </summary>
        public string Url
        {
            get
            {
                return Handler.Url;
            }

            set
            {
                Handler.Url = value;
            }
        }

        /// <summary>
        /// Gets or sets the color used to draw the label of the hyperlink when the mouse is
        /// over the control.
        /// </summary>
        [Browsable(false)]
        public Color HoverColor
        {
            get => Handler.HoverColor;
            set => Handler.HoverColor = value;
        }

        /// <summary>
        /// Gets or sets the color used to draw the label when the link has never been
        /// clicked before (i.e. the link has not been visited) and the mouse is
        /// not over the control.
        /// </summary>
        [Browsable(false)]
        public Color NormalColor
        {
            get => Handler.NormalColor;
            set => Handler.NormalColor = value;
        }

        /// <summary>
        /// Gets or sets the color used to draw the label when the mouse is not over the
        /// control and the link has already been clicked
        /// before (i.e. the link has been visited).
        /// </summary>
        [Browsable(false)]
        public Color VisitedColor
        {
            get => Handler.VisitedColor;
            set => Handler.VisitedColor = value;
        }

        /// <summary>
        /// Gets or sets whether the hyperlink has already been clicked by
        /// the user at least one time.
        /// </summary>
        [Browsable(false)]
        public bool Visited
        {
            get => Handler.Visited;
            set => Handler.Visited = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
        }

        internal new ILinkLabelHandler Handler =>
            (ILinkLabelHandler)base.Handler;

        /// <summary>
        /// Raises <see cref="LinkClicked"/> event.
        /// </summary>
        /// <param name="e"></param>
        public void RaiseLinkClicked(CancelEventArgs e)
        {
            OnLinkClicked(e);
            if (!e.Cancel)
                LinkClicked?.Invoke(this, e);
            if (!e.Cancel && UseShellExecute)
            {
                e.Cancel = true;
                AppUtils.OpenUrl(Url);
            }
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return NativePlatform.Default.CreateLinkLabelHandler(this);
        }

        /// <summary>
        /// Called when a link is clicked within the control.
        /// </summary>
        /// <param name="e">
        /// An <see cref="CancelEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnLinkClicked(CancelEventArgs e)
        {
        }
    }
}