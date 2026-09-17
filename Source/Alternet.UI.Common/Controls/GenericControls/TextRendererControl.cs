using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that can display text using <see cref="TextRendererDrawable"/>.
    /// </summary>
    public partial class TextRendererControl : GenericControl
    {
        private readonly TextRendererDrawable drawable;
        private bool isTextBackgroundTransparent = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextRendererControl"/> class.
        /// </summary>
        public TextRendererControl()
        {
            drawable = CreateDrawable();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the background of the text is transparent.
        /// If set to <c>true</c>, the background will be transparent; otherwise, it will use the control's background color.
        /// </summary>
        public virtual bool IsTextBackgroundTransparent
        {
            get => isTextBackgroundTransparent;
            set
            {
                if(isTextBackgroundTransparent == value)
                    return;
                isTextBackgroundTransparent = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets the <see cref="TextRendererDrawable"/> that is used to draw text on the control.
        /// </summary>
        [Browsable(false)]
        public TextRendererDrawable Drawable => drawable;

        /// <summary>
        /// Gets or sets the text formatting flags that control the text layout and rendering.
        /// </summary>
        public virtual TextFormatFlags Flags
        {
            get => drawable.Flags;
            set
            {
                if (drawable.Flags == value)
                    return;
                drawable.Flags = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to trim the line to the nearest word
        /// and place an ellipsis at the end of a trimmed line.
        /// </summary>
        public virtual bool WordEllipsis
        {
            get => drawable.WordEllipsis;
            set
            {
                if (drawable.WordEllipsis == value)
                    return;
                drawable.WordEllipsis = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to break the text at the end of a word.
        /// </summary>
        public virtual bool WordBreak
        {
            get => drawable.WordBreak;
            set
            {
                if (drawable.WordBreak == value)
                    return;
                drawable.WordBreak = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to add padding to the bounding rectangle.
        /// </summary>
        public virtual bool NoPadding
        {
            get => drawable.NoPadding;
            set
            {
                if (drawable.NoPadding == value)
                    return;
                drawable.NoPadding = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the text within the bounding rectangle.
        /// </summary>
        public virtual TextHorizontalAlignment TextHorizontalAlignment
        {
            get
            {
                return drawable.HorizontalAlignment;
            }

            set
            {
                if (drawable.HorizontalAlignment == value)
                    return;
                drawable.HorizontalAlignment = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the text within the bounding rectangle.
        /// </summary>
        public virtual TextVerticalAlignment TextVerticalAlignment
        {
            get
            {
                return drawable.VerticalAlignment;
            }
            set
            {
                if (drawable.VerticalAlignment == value)
                    return;
                drawable.VerticalAlignment = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            if (!drawable.Visible)
                return;

            var r = e.ClipRectangle;

            r = r.DeflatedWithPadding(Padding);

            drawable.Bounds = r;

            if (drawable.Bounds.SizeIsEmpty)
                return;

            drawable.Text = Text;
            drawable.Font = RealFont;
            drawable.ForeColor = ForeColor;
            drawable.BackColor = isTextBackgroundTransparent ? null : BackColor;
            drawable.Draw(this, e.Graphics);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TextRendererDrawable"/> class that is used to draw the control.
        /// Override this method to provide a custom implementation of the <see cref="TextRendererDrawable"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="TextRendererDrawable"/> class.</returns>
        protected virtual TextRendererDrawable CreateDrawable()
        {
            return new ();
        }
    }
}
