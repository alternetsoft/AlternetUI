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
        /// <param name="parent">Parent of the control.</param>
        public LinkLabel(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkLabel"/> class.
        /// </summary>
        public LinkLabel()
        {
            CanSelect = false;
            TabStop = false;
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
        /// Gets control handler.
        /// </summary>
        [Browsable(false)]
        public new ILinkLabelHandler Handler =>
            (ILinkLabelHandler)base.Handler;

        /// <inheritdoc/>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                value ??= StringUtils.OneSpace;
                base.Text = value;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.LinkLabel;

        /// <summary>
        /// Gets or sets the URL associated with the hyperlink.
        /// </summary>
        public virtual string Url
        {
            get
            {
                if (DisposingOrDisposed)
                    return string.Empty;
                return Handler.Url;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.Url = value;
            }
        }

        /// <summary>
        /// Gets or sets the color used to draw the label of the hyperlink when the mouse is
        /// over the control.
        /// </summary>
        [Browsable(false)]
        public virtual Color HoverColor
        {
            get
            {
                if (DisposingOrDisposed)
                    return Color.Empty;
                return Handler.HoverColor;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.HoverColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the color used to draw the label when the link has never been
        /// clicked before (i.e. the link has not been visited) and the mouse is
        /// not over the control.
        /// </summary>
        [Browsable(false)]
        public virtual Color NormalColor
        {
            get
            {
                if (DisposingOrDisposed)
                    return Color.Empty;
                return Handler.NormalColor;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.NormalColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the color used to draw the label when the mouse is not over the
        /// control and the link has already been clicked
        /// before (i.e. the link has been visited).
        /// </summary>
        [Browsable(false)]
        public virtual Color VisitedColor
        {
            get
            {
                if (DisposingOrDisposed)
                    return Color.Empty;
                return Handler.VisitedColor;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.VisitedColor = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the hyperlink has already been clicked by
        /// the user at least one time.
        /// </summary>
        [Browsable(false)]
        public virtual bool Visited
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return Handler.Visited;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.Visited = value;
            }
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
        }

        /// <summary>
        /// Raises <see cref="LinkClicked"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public void RaiseLinkClicked(CancelEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnLinkClicked(e);
            if (!e.Cancel)
                LinkClicked?.Invoke(this, e);
            if (!e.Cancel)
            {
                e.Cancel = true;
                AppUtils.OpenUrl(Url);
            }
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateLinkLabelHandler(this);
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