using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements text painting via <see cref="TextRenderer"/>.
    /// Use <see cref="TextRendererDrawable.Text"/> property to specify the text to be drawn.
    /// </summary>
    public partial class TextRendererDrawable : BaseDrawable
    {
        /// <summary>
        /// Gets or sets the text to be drawn. If set to <c>null</c>, no drawing will occur.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets the font used to draw the text. If set to <c>null</c>, the default control font will be used.
        /// </summary>
        public Font? Font { get; set; }

        /// <summary>
        /// Gets or sets the foreground color for the text. If set to <c>null</c>, the default foreground color will be used.
        /// </summary>
        public Color? ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the background color for the text. If set to <c>null</c>, the background will be transparent.
        /// </summary>
        public Color? BackColor { get; set; }

        /// <summary>
        /// Gets or sets the text formatting flags that control the text layout and rendering.
        /// </summary>
        public TextFormatFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to trim the line to the nearest word
        /// and place an ellipsis at the end of a trimmed line.
        /// </summary>
        public bool WordEllipsis
        {
            get => Flags.HasFlag(TextFormatFlags.WordEllipsis);
            set
            {
                if (value)
                    Flags |= TextFormatFlags.WordEllipsis;
                else
                    Flags &= ~TextFormatFlags.WordEllipsis;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to break the text at the end of a word.
        /// </summary>
        public bool WordBreak
        {
            get => Flags.HasFlag(TextFormatFlags.WordBreak);
            set
            {
                if (value)
                    Flags |= TextFormatFlags.WordBreak;
                else
                    Flags &= ~TextFormatFlags.WordBreak;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to add padding to the bounding rectangle.
        /// </summary>
        public bool NoPadding
        {
            get => Flags.HasFlag(TextFormatFlags.NoPadding);
            set
            {
                if (value)
                    Flags |= TextFormatFlags.NoPadding;
                else
                    Flags &= ~TextFormatFlags.NoPadding;
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the text within the bounding rectangle.
        /// </summary>
        public TextVerticalAlignment VerticalAlignment
        {
            get
            {
                if (Flags.HasFlag(TextFormatFlags.VerticalCenter))
                    return TextVerticalAlignment.Center;
                else if (Flags.HasFlag(TextFormatFlags.Bottom))
                    return TextVerticalAlignment.Bottom;
                else
                    return TextVerticalAlignment.Top;
            }
            set
            {
                var f = Flags;

                f &= ~(TextFormatFlags.VerticalCenter | TextFormatFlags.Bottom);
                switch (value)
                {
                    case TextVerticalAlignment.Center:
                        f |= TextFormatFlags.VerticalCenter;
                        break;
                    case TextVerticalAlignment.Bottom:
                        f |= TextFormatFlags.Bottom;
                        break;
                }

                Flags = f;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the text within the bounding rectangle.
        /// </summary>
        public TextHorizontalAlignment HorizontalAlignment
        {
            get
            {
                if (Flags.HasFlag(TextFormatFlags.HorizontalCenter))
                    return TextHorizontalAlignment.Center;
                else if (Flags.HasFlag(TextFormatFlags.Right))
                    return TextHorizontalAlignment.Right;
                else
                    return TextHorizontalAlignment.Left;
            }
            set
            {
                var f = Flags;

                f &= ~(TextFormatFlags.HorizontalCenter | TextFormatFlags.Right);
                switch (value)
                {
                    case TextHorizontalAlignment.Center:
                        f |= TextFormatFlags.HorizontalCenter;
                        break;
                    case TextHorizontalAlignment.Right:
                        f |= TextFormatFlags.Right;
                        break;
                }

                Flags = f;
            }
        }

        /// <inheritdoc/>
        protected override void OnDraw(AbstractControl control, Graphics dc)
        {
            if (!Visible || Bounds.SizeIsEmpty)
                return;

            TextRenderer.DrawText(
                dc,
                Text,
                Font ?? control.Font,
                Bounds,
                ForeColor ?? DefaultColors.ControlForeColor.GetColor(IsDark()),
                BackColor,
                Flags);

            bool IsDark()
            {
                return control?.IsDarkBackground ?? false;
            }
        }
    }
}


/*
        /// <summary>
        /// Aligns the text on the top of the bounding rectangle.
        /// </summary>
#pragma warning disable
        Top = 0,
#pragma warning restore

        /// <summary>
        /// Centers the text horizontally within the bounding rectangle.
        /// </summary>
        HorizontalCenter = 1,

        /// <summary>
        /// Aligns the text on the right side of the clipping area.
        /// </summary>
        Right = 2,

        /// <summary>
        /// Centers the text vertically, within the bounding rectangle.
        /// </summary>
        VerticalCenter = 4,

        /// <summary>
        /// Aligns the text on the bottom of the bounding rectangle. Applied only
        /// when the text is a single line.
        /// </summary>
        Bottom = 8,
*/