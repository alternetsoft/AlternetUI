using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents the font information for Maui controls.
    /// </summary>
    public struct MauiFontInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MauiFontInfo"/> struct with the specified font.
        /// </summary>
        public static Func<Alternet.Drawing.Font, MauiFontInfo>? FontToMaui;

        /// <summary>
        /// Creates a new instance of the <see cref="MauiFontInfo"/> structure.
        /// </summary>
        public MauiFontInfo()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MauiFontInfo"/> structure
        /// using the specified <see cref="Alternet.Drawing.Font"/>.
        /// </summary>
        /// <param name="font">The font to use for the MauiFontInfo instance.</param>
        public MauiFontInfo(Alternet.Drawing.Font? font)
        {
            Assign(font);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MauiFontInfo"/> structure using the specified
        /// font family, font size and font attributes.
        /// </summary>
        /// <param name="fontFamily"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontAttributes"></param>
        public MauiFontInfo(string? fontFamily, double fontSize, FontAttributes fontAttributes)
        {
            FontFamily = fontFamily;
            FontSize = fontSize;
            FontAttributes = fontAttributes;
        }

        /// <summary>
        /// Assigns the specified <see cref="Alternet.Drawing.Font"/> to
        /// the current instance of the <see cref="MauiFontInfo"/> structure.
        /// </summary>
        /// <param name="font">The font to assign.</param>
        public void Assign(Alternet.Drawing.Font? font)
        {
            font ??= Alternet.UI.Control.DefaultFont;

            if (FontToMaui is not null)
            {
                MauiFontInfo fi = FontToMaui(font);
                FontFamily = fi.FontFamily;
                FontSize = fi.FontSize;
                FontAttributes = fi.FontAttributes;
                return;
            }

            FontAttributes fa = new();

            if (font.IsBold)
                fa |= FontAttributes.Bold;
            if (font.IsItalic)
                fa |= FontAttributes.Italic;

            FontAttributes = fa;
            FontSize = font.Size * 1.333;
            FontFamily = font.Name;
        }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        public string? FontFamily;

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        public double FontSize;

        /// <summary>
        /// Gets or sets the font attributes.
        /// </summary>
        public FontAttributes FontAttributes;
    }
}
