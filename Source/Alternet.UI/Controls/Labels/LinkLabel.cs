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
        private string? text;

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
            this.text = text;
        }

        /// <summary>
        /// Occurs when a link is clicked within the control.
        /// </summary>
        public event CancelEventHandler? LinkClicked;

        /// <summary>
        /// Gets or sets whether to use generic or native control for <see cref="LinkLabel"/>.
        /// </summary>
        public static bool UseGenericControl
        {
            get => Native.LinkLabel.UseGenericControl;
            set => Native.LinkLabel.UseGenericControl = value;
        }

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
                return text ?? string.Empty;
            }

            set
            {
                if (text == value)
                    return;
                text = value;
                NativeControl.Text = text ?? " ";
                OnTextChanged(EventArgs.Empty);
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
                return NativeControl.Url;
            }

            set
            {
                NativeControl.Url = value;
            }
        }

        /// <summary>
        /// Gets or sets the color used to draw the label of the hyperlink when the mouse is
        /// over the control.
        /// </summary>
        [Browsable(false)]
        public Color HoverColor
        {
            get => NativeControl.HoverColor;
            set => NativeControl.HoverColor = value;
        }

        /// <summary>
        /// Gets or sets the color used to draw the label when the link has never been
        /// clicked before (i.e. the link has not been visited) and the mouse is
        /// not over the control.
        /// </summary>
        [Browsable(false)]
        public Color NormalColor
        {
            get => NativeControl.NormalColor;
            set => NativeControl.NormalColor = value;
        }

        /// <summary>
        /// Gets or sets the color used to draw the label when the mouse is not over the
        /// control and the link has already been clicked
        /// before (i.e. the link has been visited).
        /// </summary>
        [Browsable(false)]
        public Color VisitedColor
        {
            get => NativeControl.VisitedColor;
            set => NativeControl.VisitedColor = value;
        }

        /// <summary>
        /// Gets or sets whether the hyperlink has already been clicked by
        /// the user at least one time.
        /// </summary>
        [Browsable(false)]
        public bool Visited
        {
            get => NativeControl.Visited;
            set => NativeControl.Visited = value;
        }

        internal new Native.LinkLabel NativeControl =>
             (Native.LinkLabel)base.NativeControl;

        internal new NativeLinkLabelHandler Handler =>
            (NativeLinkLabelHandler)base.Handler;

        internal void RaiseLinkClicked(CancelEventArgs e)
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
        internal override ControlHandler CreateHandler()
        {
            return new NativeLinkLabelHandler();
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