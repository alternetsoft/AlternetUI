using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal class StyledTextWithFontAndColor : SimpleStyledText
    {
        private Font? font;
        private FontStyle? fontStyle;
        private Color? foregroundColor;
        private Color? backgroundColor;
        private Brush? foregroundBrush;

        public StyledTextWithFontAndColor()
            : base()
        {
        }

        public StyledTextWithFontAndColor(object s)
            : base(s)
        {
        }

        public StyledTextWithFontAndColor(object s, FontStyle? fontStyle, Brush? foreBrush)
            : base(s)
        {
            this.fontStyle = fontStyle;
            foregroundBrush = foreBrush;
        }

        public StyledTextWithFontAndColor(
            object s,
            FontStyle? fontStyle,
            Color? foreColor,
            Color? backColor)
            : base(s)
        {
            this.fontStyle = fontStyle;
            foregroundColor = foreColor;
            backgroundColor = backColor;
        }

        public StyledTextWithFontAndColor(object s, Font? font, Brush? foreBrush)
            : base(s)
        {
            this.font = font;
            foregroundBrush = foreBrush;
        }

        public StyledTextWithFontAndColor(object s, Font? font, Color? foreColor, Color? backColor)
            : base(s)
        {
            this.font = font;
            foregroundColor = foreColor;
            backgroundColor = backColor;
        }

        public virtual Color? ForegroundColor
        {
            get
            {
                return foregroundColor;
            }

            set
            {
                foregroundColor = value;
            }
        }

        public virtual Color? BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                backgroundColor = value;
            }
        }

        public virtual Brush? ForegroundBrush
        {
            get
            {
                return foregroundBrush;
            }

            set
            {
                foregroundBrush = value;
            }
        }

        public virtual Font? Font
        {
            get
            {
                return font;
            }

            set
            {
                if (font == value)
                    return;
                font = value;
                Changed();
            }
        }

        public virtual FontStyle? FontStyle
        {
            get
            {
                return fontStyle;
            }

            set
            {
                if (fontStyle == value)
                    return;
                fontStyle = value;
                Changed();
            }
        }

        public override void Changed()
        {
            base.Changed();
        }

        internal override Font SafeFont(Font? font)
        {
            if (this.font is not null)
                return this.font;

            var baseFont = base.SafeFont(font);

            if (fontStyle is null)
                return baseFont;
            return baseFont.WithStyle(fontStyle.Value);
        }

        internal override Brush SafeForeBrush(Brush? brush)
        {
            return foregroundBrush ?? base.SafeForeBrush(brush);
        }

        internal override Color SafeForeColor(Color? foreColor)
        {
            return foregroundColor ?? base.SafeForeColor(foreColor);
        }

        internal override Color SafeBackColor(Color? backColor)
        {
            return backgroundColor ?? base.SafeBackColor(backColor);
        }
    }
}
