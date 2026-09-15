using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a structure that contains information about a font that is relative to another font,
    /// including its base font, real font, relative style, and relative size.
    /// <para>
    /// Use <see cref="BaseFont"/> to get or set the base font,
    /// which is the font that the relative font is based on.
    /// If the base font is not specified, the resulting font will use default font as the base font.
    /// </para>
    /// <para>
    /// Use <see cref="ResultFont"/> to get the resulting font that is created by applying
    /// the relative style and relative size to the base font.
    /// </para>
    /// <para>
    /// Use <see cref="RelativeStyle"/> to get or set the relative style of the font, which
    /// is a combination of <see cref="FontStyle"/>. If the relative style is not specified, 
    /// the resulting font will have the same style as the base font. If any of the <see cref="FontStyle"/> values are specified,
    /// they will be applied to the base font to create the resulting font.
    /// </para>
    /// <para>
    /// Use <see cref="RelativeSize"/> to get or set the relative size of the font, which is a value that is applied
    /// to the base font to create the resulting font. If the relative size is not specified,
    /// the resulting font will have the same size as the base font. If a relative size is specified,
    /// it will be applied to the base font to create the resulting font.
    /// </para>
    /// </summary>
    public partial struct RelativeFontInfo
    {
        private Font baseFont;
        private Font? resultFont;
        private FontStyle relativeStyle;
        private RelativeFontSize? relativeSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeFontInfo"/> struct with default values.
        /// </summary>
        public RelativeFontInfo()
            : this(AbstractControl.DefaultFont)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeFontInfo"/> struct with the specified
        /// base font and an event handler for changes.
        /// </summary>
        /// <param name="baseFont">The base font.</param>
        /// <param name="changed">The event handler for changes.</param>
        public RelativeFontInfo(Font baseFont, Action<EventArgs> changed)
        {
            this.baseFont = baseFont;
            this.Changed += (s,e) => changed(e);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeFontInfo"/> struct with the specified
        /// base font and an action to be invoked when the font changes.
        /// </summary>
        /// <param name="baseFont">The base font.</param>
        /// <param name="changed">The action to be invoked when the font changes.</param>
        public RelativeFontInfo(Font baseFont, Action changed)
        {
            this.baseFont = baseFont;
            this.Changed += (s, e) => changed();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeFontInfo"/> struct with the specified base font.
        /// </summary>
        /// <param name="baseFont">The base font.</param>
        public RelativeFontInfo(Font baseFont)
        {
            this.baseFont = baseFont;
        }

        /// <summary>
        /// Occurs when the relative font information changes, such as when the base font,
        /// relative style, or relative size is modified.
        /// </summary>
        public event EventHandler? Changed;

        /// <summary>
        /// Gets or sets the base font, which is the font that the relative font is based on.
        /// If the base font is not specified, the resulting font will use default font as the base font.
        /// </summary>
        public Font BaseFont
        {
            readonly get => baseFont;
            set
            {
                value ??= AbstractControl.DefaultFont;
                if (Font.AreEqual(baseFont, value))
                    return;
                baseFont = value;
                OnChanged();
            }
        }

        /// <summary>
        /// Gets the resulting font, which is the font that is created by applying
        /// the relative style and relative size to the base font.
        /// </summary>
        public Font ResultFont
        {
            get
            {
                if (resultFont is not null)
                    return resultFont;

                if (RelativeSize is null)
                {
                    if (relativeStyle == 0)
                        resultFont = baseFont;
                    else
                        resultFont = baseFont.WithStyle(relativeStyle);
                    return resultFont;
                }
                else
                {
                    if (relativeStyle == 0)
                    {
                        resultFont = new(baseFont, RelativeSize.Value);
                    }
                    else
                    {
                        resultFont = new(baseFont, RelativeSize.Value, relativeStyle);
                    }

                    return resultFont;
                }
            }
        }

        /// <summary>
        /// Gets or sets the relative style of the font, which
        /// is a combination of <see cref="FontStyle"/>
        /// values that are applied to the base font to create the resulting font.
        /// If the relative style is not specified,
        /// the resulting font will have the same style as the base font.
        /// If any of the <see cref="FontStyle"/> values are specified,
        /// they will be applied to the base font to create the resulting font.
        /// </summary>
        public FontStyle RelativeStyle
        {
            readonly get
            {
                return relativeStyle;
            }
            set
            {
                if (relativeStyle == value)
                    return;
                relativeStyle = value;
                OnChanged();
            }
        }

        /// <summary>
        /// Gets or sets the relative size of the font, which is a value that is applied
        /// to the base font to create the resulting font.
        /// If the relative size is not specified,
        /// the resulting font will have the same size as the base font. If a relative size is specified,
        /// it will be applied to the base font to create the resulting font.
        /// </summary>
        public RelativeFontSize? RelativeSize
        {
            readonly get => relativeSize;
            set
            {
                if (relativeSize == value)
                    return;
                relativeSize = value;
                OnChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the relative font is bold,
        /// which is determined by checking if the <see cref="FontStyle.Bold"/>
        /// flag is set in the <see cref="RelativeStyle"/> property.
        /// </summary>
        public readonly bool IsRelativeBold => relativeStyle.HasFlag(FontStyle.Bold);

        /// <summary>
        /// Gets a value indicating whether the relative font has a specified relative size.
        /// </summary>
        public readonly bool HasRelativeSize => relativeSize.HasValue;

        /// <summary>
        /// Gets a value indicating whether the relative font has a specified relative style.
        /// </summary>
        public readonly bool HasRelativeStyle() => relativeStyle != 0;

        /// <summary>
        /// Determines whether the relative font has a specified relative style.
        /// </summary>
        /// <param name="style">The font style to check.</param>
        /// <returns>true if the relative font has the specified style; otherwise, false.</returns>
        public readonly bool HasRelativeStyle(FontStyle style) => relativeStyle.HasFlag(style);

        /// <summary>
        /// Sets a value indicating whether the relative font is bold.
        /// </summary>
        /// <param name="value">true to set the relative font as bold; otherwise, false.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRelativeBold(bool value)
        {
            SetRelativeStyle(value, FontStyle.Bold);
        }

        /// <summary>
        /// Sets a value indicating whether the relative font has a specified relative style.
        /// </summary>
        /// <param name="value">true to set the relative font with the specified style; otherwise, false.</param>
        /// <param name="style">The font style to set or unset.</param>
        public void SetRelativeStyle(bool value, FontStyle style)
        {
            var oldValue = relativeStyle.HasFlag(style);

            if (oldValue == value)
                return;

            if (value)
                relativeStyle |= style;
            else
                relativeStyle &= ~style;

            OnChanged();
        }

        /// <summary>
        /// Raises the <see cref="Changed"/> event to notify subscribers that the relative font information has changed.
        /// </summary>
        public void OnChanged()
        {
            resultFont = null;
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
