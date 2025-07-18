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
    public partial class LinkLabel : Label
    {
        /// <summary>
        /// Gets or sets default visited hyperlink color which is used in the control.
        /// Default is <c>null</c>. If this value
        /// is not assigned, <see cref="DefaultNormalColor"/> is used.
        /// </summary>
        public static LightDarkColor? DefaultVisitedColor;

        /// <summary>
        /// Gets or sets default hyperlink hover color which is used in the control.
        /// Default is <c>null</c>. If this value
        /// is not assigned, <see cref="DefaultNormalColor"/> is used.
        /// </summary>
        public static LightDarkColor? DefaultHoverColor;

        private static LightDarkColor? defaultNormalColor;

        private string? url;
        private Color? hoverColor;
        private Color? visitedColor;
        private Color? normalColor;
        private bool visited;

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
            HorizontalAlignment = HorizontalAlignment.Left;
            CanSelect = false;
            TabStop = false;
            IsUnderline = true;
            ParentForeColor = false;
            Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkLabel"/> class with the specified text.
        /// </summary>
        public LinkLabel(string? text)
            : this()
        {
            Text = text ?? string.Empty;
        }

        /// <summary>
        /// Occurs when a link is clicked within the control.
        /// </summary>
        public event CancelEventHandler? LinkClicked;

        /// <summary>
        /// Gets or sets default normal hyperlink color which is used in the control.
        /// </summary>
        public static LightDarkColor DefaultNormalColor
        {
            get
            {
                return defaultNormalColor ??= new LightDarkColor((0, 0, 255), (86, 156, 198));
            }

            set
            {
                defaultNormalColor = value;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.LinkLabel;

        /// <summary>
        /// Gets or sets the URL associated with the hyperlink.
        /// </summary>
        public virtual string? Url
        {
            get
            {
                return url;
            }

            set
            {
                url = value;
            }
        }

        /// <summary>
        /// Gets or sets the color used to draw the label of the hyperlink when the mouse is
        /// over the control.
        /// </summary>
        [Browsable(false)]
        public virtual Color? HoverColor
        {
            get
            {
                return hoverColor;
            }

            set
            {
                if(hoverColor == value)
                    return;
                hoverColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color used to draw the label when the link has never been
        /// clicked before (i.e. the link has not been visited) and the mouse is
        /// not over the control.
        /// </summary>
        [Browsable(false)]
        public virtual Color? NormalColor
        {
            get
            {
                return normalColor;
            }

            set
            {
                if(normalColor == value)
                    return;
                normalColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color used to draw the label when the mouse is not over the
        /// control and the link has already been clicked
        /// before (i.e. the link has been visited).
        /// </summary>
        [Browsable(false)]
        public virtual Color? VisitedColor
        {
            get
            {
                return visitedColor;
            }

            set
            {
                if(visitedColor == value)
                    return;
                visitedColor = value;
                Invalidate();
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
                return visited;
            }

            set
            {
                if(visited == value)
                    return;
                visited = value;
                Invalidate();
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
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            Post(() =>
            {
                RaiseLinkClicked(new CancelEventArgs());
            });
        }

        /// <inheritdoc/>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            Post(() =>
            {
                RaiseLinkClicked(new CancelEventArgs());
            });
        }

        /// <inheritdoc/>
        protected override Color GetLabelForeColor(VisualControlState state)
        {
            if (state == VisualControlState.Hovered)
            {
                var realHoveredColor = HoverColor ?? DefaultHoverColor;
                if (realHoveredColor is not null)
                    return realHoveredColor;
            }

            if (Visited)
            {
                var realVisitedColor = VisitedColor ?? DefaultVisitedColor;
                if (realVisitedColor is not null)
                    return realVisitedColor;
            }

            var realNormalColor = NormalColor ?? DefaultNormalColor ?? LightDarkColors.Blue;

            return realNormalColor;
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