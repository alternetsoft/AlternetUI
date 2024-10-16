using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines style of the drawable element.
    /// </summary>
    public class DrawableElementStyle : ImmutableObject
    {
        /// <summary>
        /// Gets name of the parent proprty.
        /// </summary>
        public const string PropNameParent = nameof(Parent);

        /// <summary>
        /// Gets name of the font proprty.
        /// </summary>
        public const string PropNameFont = nameof(Font);

        /// <summary>
        /// Gets name of the font style proprty.
        /// </summary>
        public const string PropNameFontStyle = nameof(FontStyle);

        /// <summary>
        /// Gets name of the foreground brush proprty.
        /// </summary>
        public const string PropNameForegroundBrush = nameof(ForegroundBrush);

        /// <summary>
        /// Gets name of the foreground color proprty.
        /// </summary>
        public const string PropNameForegroundColor = nameof(ForegroundColor);

        /// <summary>
        /// Gets name of the background color proprty.
        /// </summary>
        public const string PropNameBackgroundColor = nameof(BackgroundColor);

        /// <summary>
        /// Gets or sets default foreground color of the styled text.
        /// </summary>
        public static Color DefaultForegroundColor = Color.Black;

        /// <summary>
        /// Gets or sets default background color of the styled text.
        /// </summary>
        public static Color DefaultBackgroundColor = Color.White;

        /// <summary>
        /// Gets or sets default attributes of the styled text.
        /// </summary>
        public static DrawableElementStyle Default = new();

        private Font? font;
        private Brush? foreBrush;
        private Color? foreColor;
        private Color? backColor;
        private FontStyle? fontStyle;
        private DrawableElementStyle? parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableElementStyle"/> class.
        /// </summary>
        public DrawableElementStyle()
        {
        }

        /// <summary>
        /// Occurs when property is changed.
        /// </summary>
        public event EventHandler? Changed;

        /// <summary>
        /// Gets or sets parent style.
        /// </summary>
        public virtual DrawableElementStyle? Parent
        {
            get
            {
                return parent;
            }

            set
            {
                if (parent == value)
                    return;
                parent = value;
                RaiseChanged(PropNameParent);
            }
        }

        /// <summary>
        /// Gets or sets font style.
        /// </summary>
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
                RaiseChanged(PropNameFontStyle);
            }
        }

        /// <summary>
        /// Gets or sets font.
        /// </summary>
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
                RaiseChanged(PropNameFont);
            }
        }

        /// <summary>
        /// Gets or sets foreground brush.
        /// </summary>
        public virtual Brush? ForegroundBrush
        {
            get
            {
                return foreBrush;
            }

            set
            {
                if (foreBrush == value)
                    return;
                foreBrush = value;
                RaiseChanged(PropNameForegroundBrush);
            }
        }

        /// <summary>
        /// Gets or sets foreground color.
        /// </summary>
        public virtual Color? ForegroundColor
        {
            get
            {
                return foreColor;
            }

            set
            {
                if (foreColor == value)
                    return;
                foreColor = value;
                RaiseChanged(PropNameForegroundColor);
            }
        }

        /// <summary>
        /// Gets or sets background color.
        /// </summary>
        public virtual Color? BackgroundColor
        {
            get
            {
                return backColor;
            }

            set
            {
                if (backColor == value)
                    return;
                backColor = value;
                RaiseChanged(PropNameBackgroundColor);
            }
        }

        /// <summary>
        /// Gets real foreground brush.
        /// </summary>
        [Browsable(false)]
        public virtual Brush? RealForeBrush
        {
            get
            {
                return foreBrush ?? Parent?.ForegroundBrush;
            }
        }

        /// <summary>
        /// Gets real font style.
        /// </summary>
        [Browsable(false)]
        public FontStyle? RealFontStyle
        {
            get
            {
                return FontStyle ?? Parent?.FontStyle;
            }
        }

        /// <summary>
        /// Gets real font.
        /// </summary>
        [Browsable(false)]
        public virtual Font RealFont
        {
            get
            {
                var result = Font ?? Parent?.Font ?? AbstractControl.DefaultFont;
                var realFontStyle = RealFontStyle;

                if (realFontStyle is null)
                    return result;
                return result.WithStyle(realFontStyle.Value);
            }
        }

        /// <summary>
        /// Gets real foreground color.
        /// </summary>
        [Browsable(false)]
        public virtual Color? RealForeColor
        {
            get
            {
                return foreColor ?? Parent?.ForegroundColor;
            }
        }

        /// <summary>
        /// Gets real background color.
        /// </summary>
        [Browsable(false)]
        public virtual Color? RealBackColor
        {
            get
            {
                return backColor ?? Parent?.BackgroundColor;
            }
        }

        /// <summary>
        /// Raises <see cref="Changed"/> event and <see cref="OnChanged"/> method.
        /// </summary>
        /// <param name="propName"></param>
        public void RaiseChanged(string? propName)
        {
            OnChanged(propName);
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public virtual DrawableElementStyle Clone()
        {
            DrawableElementStyle result = new();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Assigns properties of this object with properties of another object.
        /// </summary>
        /// <param name="assignFrom">Source of the property values.</param>
        public virtual void Assign(DrawableElementStyle assignFrom)
        {
            font = assignFrom.font;
            foreBrush = assignFrom.foreBrush;
            foreColor = assignFrom.foreColor;
            backColor = assignFrom.backColor;
            fontStyle = assignFrom.fontStyle;
            parent = assignFrom.parent;
            RaiseChanged(null);
        }

        /// <summary>
        /// Called when <see cref="Changed"/> event is raised.
        /// </summary>
        /// <param name="propName">Name of the changed property.</param>
        protected virtual void OnChanged(string? propName)
        {
        }
    }
}
